using System.Diagnostics;
using System.IO;

namespace ImageCombine
{
    public class Utils
    {
        public static string GetExecuteDir()
        {
            return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        }

        public static string GetRealPath(string filePath, string basePath = null)
        {
            if (Path.IsPathRooted(filePath))
            {
                return filePath;
            }
            return Path.GetFullPath(Path.Combine(string.IsNullOrWhiteSpace(basePath) ? GetExecuteDir() : basePath, filePath));
        }

        /// <summary>
        /// 打开-资源管理器
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool OpenExplorer(string filePath)
        {
            string argType = null;
            // 如果是文件, 则选中
            if (File.Exists(filePath))
            {
                argType = "select";
            }
            else if (Directory.Exists(filePath))
            {
                // 如果是文件夹, 就打开
                argType = "open";
            }
            else
            {
                // 如果文件目录不存在，但是上级文件夹存在，就打开其上级文件夹
                string tempPath = Path.GetDirectoryName(filePath);
                if (Directory.Exists(tempPath))
                {
                    argType = "open";
                    filePath = tempPath;
                }
                else
                {
                    // 如果路径不存在, 就打开可执行程序所在目录
                    argType = "open";
                    filePath = GetExecuteDir();
                }
            }
            var info = new ProcessStartInfo("explorer.exe", string.Format("/{0}, \"{1}\"", argType, filePath))
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = Directory.Exists(filePath) ? filePath : Path.GetDirectoryName(filePath),
            };
            bool isSuccess = false;
            using (var process = Process.Start(info))
            {
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }
    }
}
