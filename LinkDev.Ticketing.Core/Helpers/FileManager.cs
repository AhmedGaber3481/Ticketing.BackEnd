namespace LinkDev.Ticketing.Core.Helpers
{
    public class FileManager
    {
        private readonly DirectoryManager _directoryManager;
        public FileManager(DirectoryManager directoryManager)
        {
            _directoryManager = directoryManager;
        }

        public string? WriteFile(int directoryKey, string fileName, byte[] content, out string fileUrl)
        {
            fileUrl = string.Empty;
            string? dirPath = _directoryManager.CreateDirectory(directoryKey);
            string? filePath = null;
            if (!string.IsNullOrEmpty(dirPath))
            {
                filePath = dirPath + "\\" + fileName;
                File.WriteAllBytes(filePath, content);

                fileUrl = filePath.Substring(_directoryManager.RootPath.Length).Replace("\\", "/");
            }

            return filePath;
        }

        public string? WriteFile(int directoryKey, string fileName, string content)
        {
            string? dirPath = _directoryManager.CreateDirectory(directoryKey);
            string? filePath = null;
            if (!string.IsNullOrEmpty(dirPath))
            {
                filePath = dirPath + "\\" + fileName;
                File.WriteAllText(filePath, content);
            }

            return filePath;
        }

    }
}
