using System.IO;

namespace FolderCleanupService.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Move (delete) file to recycle bin.
        /// ATTENTION: NETWORK FILES DO NOT GO TO THE RECYCLE BIN
        /// </summary>
        /// <param name="file">Full path of the file to delete</param>
        private static void MoveFileToRecycleBin(this string file)
        {
            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(file,
                Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
        }

        /// <summary>
        /// Move (delete) folder to recycle bin.
        /// ATTENTION: NETWORK FOLDERS DO NOT GO TO THE RECYCLE BIN
        /// </summary>
        /// <param name="folder">Full path of the folder to delete</param>
        private static void MoveFolderToRecycleBin(this string folder)
        {
            Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(folder,
                Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
        }

        /// <summary>
        /// Delete file.
        /// </summary>
        /// <param name="file">Full path of file to delete</param>
        /// <param name="useRecycleBin">Move file to recylce bin</param>
        public static void DeleteFile(this string file, bool useRecycleBin)
        {
            if (useRecycleBin == true)
            {
                file.MoveFileToRecycleBin();
            }
            else
            {
                File.Delete(file);
            }
        }

        /// <summary>
        /// Delete folder.
        /// </summary>
        /// <param name="folder">Full path of the folder to delete</param>
        /// <param name="useRecycleBin">Move file to recycle bin</param>
        public static void DeleteFolder(this string folder, bool useRecycleBin)
        {
            if (useRecycleBin == true)
            {
                folder.MoveFolderToRecycleBin();
            }
            else
            {
                Directory.Delete(folder, true);
            }
        }
    }
}
