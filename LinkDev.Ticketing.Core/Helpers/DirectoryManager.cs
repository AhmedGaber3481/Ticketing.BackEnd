using LinkDev.Ticketing.Core.Enums;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace LinkDev.Ticketing.Core.Helpers
{
    public class DirectoryManager
    {
        private int maxFilesInDirectory = 1000;
        private string rootPath;
        private DirectoryRolloutInterval rolloutInterval = DirectoryRolloutInterval.Month;

        public string RootPath
        {
            get { return rootPath; }
        }
        
        public DirectoryManager(IConfiguration configuration)
        {
            rootPath = configuration["AttachmentSettings:rootPath"];
            if(int.TryParse(configuration["AttachmentSettings:maxFilesInDirectory"], out int _maxFilesInDirectory))
            {
                maxFilesInDirectory = _maxFilesInDirectory;
            }
            if (Enum.TryParse(configuration["AttachmentSettings:rolloutInterval"],true ,out DirectoryRolloutInterval _rolloutInterval))
            {
                rolloutInterval = _rolloutInterval;
            }
        }

        public string? CreateDirectory(int key)
        {
            string? dirPath = null;
            string? baseDirPath = CreateBaseDirectory();
            if (!string.IsNullOrEmpty(baseDirPath))
            {
                string folderName =  (key / maxFilesInDirectory).ToString();
                dirPath = baseDirPath + "\\" + folderName;
                if (!Directory.Exists(dirPath)) 
                {
                    Directory.CreateDirectory(dirPath);
                }
            }
            return dirPath;
        }

        public string? CreateBaseDirectory()
        {
            string? directoryPath = null;
            string? directoryName = null;

            switch (rolloutInterval)
            {
                case DirectoryRolloutInterval.Day:
                    directoryName = DateTime.Now.ToString("yyyyMMdd");
                    break;
                case DirectoryRolloutInterval.Month:
                    directoryName = DateTime.Now.ToString("yyyyMM");
                    break;
                case DirectoryRolloutInterval.Year:
                    directoryName = DateTime.Now.ToString("yyyy");
                    break;
                default:
                    break;
            }

            if (directoryName != null)
            {
                StringBuilder pathBuilder = new StringBuilder();
                pathBuilder.AppendFormat("{0}\\{1}", rootPath, directoryName);
                string path = pathBuilder.ToString();

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                directoryPath = path;
            }
            return directoryPath;
        }
    }
}
