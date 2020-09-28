using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FolderCleanupService.Extensions;
using FolderCleanupService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FolderCleanupService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly ServiceSetting _serviceSettings;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceSettings = _configuration.GetSection("Service").Get<ServiceSetting>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Start folder cleanup at: {time}", DateTimeOffset.Now);

                CleanupFolders();

                _logger.LogInformation("Finished folder cleanup at: {time}", DateTimeOffset.Now);

                await Task.Delay(_serviceSettings.IntervalInMilliseconds, stoppingToken);
            }
        }

        private void CleanupFolders()
        {
            // Get all folders from configuration
            List<Folder> folders = _configuration.GetSection("Folders").Get<List<Folder>>();
            
            // Run cleanup for each individual folder
            foreach (Folder folder in folders)
            {
                _logger.LogInformation("Start cleanup for folder: {folder}", folder.Path);

                if (Directory.Exists(folder.Path) == true)
                {
                    CleanupFolder(folder.Path, folder.MaximumFileAgeInDays, folder.UseRecycleBin, folder.Recursive); 
                }

                _logger.LogInformation("Finished cleanup for folder: {folder}", folder.Path);
            }
        }

        private void CleanupFolder(string folderPath, int maximumFileAgeInDays, bool useRecycleBin, bool recursive)
        {
            // Recursive call, to run through all sub folders
            if (recursive == true)
            {
                var subDirectories = Directory.GetDirectories(folderPath);
                foreach (string subDirectory in subDirectories)
                {
                    _logger.LogInformation("Processing sub directory {subdirectory} of {folder}", subDirectory, folderPath);

                    CleanupFolder(subDirectory, maximumFileAgeInDays, useRecycleBin, recursive);
                } 
            }

            // Get all files of current folder
            var files = Directory.GetFiles(folderPath);

            // Delete file if older than max file age in days
            foreach (string file in files)
            {
                DateTime fileLastAccessed = File.GetLastAccessTime(file);
                if (DateTime.Now.AddDays(-maximumFileAgeInDays) >= fileLastAccessed)
                {
                    _logger.LogInformation("Delete file: {file}", file);
                    file.DeleteFile(useRecycleBin);
                }
            }

            // Delete empty folder
            if (Directory.GetFiles(folderPath).Any() == false)
            {
                _logger.LogInformation("Delete folder: {folder}", folderPath);
                folderPath.DeleteFolder(useRecycleBin);
            }
        }
    }
}
