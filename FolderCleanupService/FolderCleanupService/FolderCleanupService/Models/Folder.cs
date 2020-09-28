namespace FolderCleanupService.Models
{
    public class Folder
    {
        public string Path { get; set; }
        public int MaximumFileAgeInDays { get; set; } = 7;
        public bool UseRecycleBin { get; set; } = true;
        public bool Recursive { get; set; } = false;
    }
}
