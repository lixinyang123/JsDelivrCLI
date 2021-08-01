using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using JSDelivrCLI.Common;
using JSDelivrCLI.Models;

namespace JSDelivrCLI.Services
{
    public class CDNService
    {
        private readonly string api = "https://data.jsdelivr.com/v1/package/npm/";
        private readonly string url = "https://cdn.jsdelivr.net/npm/";
        private readonly string searchApi = "https://registry.npmjs.org/-/v1/search?text=";
        private List<string> errorList;

        private readonly HttpClient httpClient;

        public CDNService()
        {
            httpClient = new HttpClient();
            errorList = new List<string>();
        }

        public void Search(string libraryName)
        {

        }

        // 获取包版本信息
        public async Task<LibraryVersion> GetLibraryVersions(string libraryName)
        {
            string path = Path.Combine(api, libraryName);
            HttpResponseMessage responseMessage = await httpClient.GetAsync(path);
            string jsonStr = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LibraryVersion>(jsonStr);
        }

        // 获取包文件列表
        public async Task<Library> GetFileList(ConfigPara para)
        {
            LibraryVersion version = await GetLibraryVersions(para.Name);
            if(string.IsNullOrEmpty(para.Version))
            {
                ConsoleTool.WriteColorful($"Use latest version {version.Tag.Latest}\n", ConsoleColor.Green);
                para.Version = version.Tag.Latest;
            }

            if(!version.Versions.Contains(para.Version))
            {
                ConsoleTool.WriteColorful($"Can't find version {version.Tag.Latest}", ConsoleColor.Red);
                return null;
            }

            string path = Path.Combine(api, para.ToString());
            Console.WriteLine("Get library info...");
            
            HttpResponseMessage responseMessage = await httpClient.GetAsync(path);
            string jsonStr = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Library>(jsonStr);
        }

        // 获取包清单并下载
        public async Task<bool> Download(ConfigPara para, string dir = "")
        {
            Library package = await GetFileList(para);

            if(package == null)
                return false;
            
            ConsoleTool.WriteColorful("Start downloading...\n", ConsoleColor.Blue);
            errorList.Clear();
            bool flag = SaveFile(dir, para, string.Empty, package.Files);

            errorList.ForEach(i => 
            {
                ConsoleTool.WriteColorful($"\nError: {i} download faled", ConsoleColor.Red);
            });
            return flag;
        }

        // 下载包
        private bool SaveFile(string saveDir, ConfigPara para, string parentPath, List<LibraryFile> packageFile)
        {
            List<Task> tasks = new();

            packageFile.ForEach(file => 
            {
                tasks.Add(Task.Run(() =>
                {
                    string path = Path.Combine(parentPath, file.Name);
                    if (file.Type == "directory")
                    {
                        SaveFile(saveDir, para, path, file.Files);
                    }
                    else
                    {
                        string dirName = Path.Combine(saveDir, para.Name, parentPath);
                        string localPath = Path.Combine(saveDir, para.Name, path);
                        string remotePath = Path.Combine(url, para.ToString(), path);

                        try
                        {
                            if (File.Exists(localPath))
                                return;

                            HttpResponseMessage responseMessage = httpClient.GetAsync(remotePath).Result;
                            string content = responseMessage.Content.ReadAsStringAsync().Result;

                            Console.WriteLine($"Writefile {localPath}");
                            if (!Directory.Exists(dirName))
                                Directory.CreateDirectory(dirName);

                            File.WriteAllText(localPath, content);
                        }
                        catch (Exception)
                        {
                            ConsoleTool.WriteColorful($"Faled {localPath}", ConsoleColor.Red);
                            errorList.Add(remotePath);
                        }
                    }
                }));
            });

            Task.WaitAll(tasks.ToArray());

            return errorList.Count == 0;
        }
    }
}