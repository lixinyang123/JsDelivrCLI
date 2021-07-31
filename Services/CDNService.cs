using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using JSDelivrCLI.Models;

namespace JSDelivrCLI.Services
{
    public class CDNService
    {
        private readonly string api = "https://data.jsdelivr.com/v1/package/npm/";
        private readonly string url = "https://cdn.jsdelivr.net/npm/";

        private readonly HttpClient httpClient;

        public CDNService()
        {
            httpClient = new HttpClient();
        }

        // 获取包版本信息
        public PackageVersion GetLibraryVersions(string libraryName)
        {
            string path = Path.Combine(api, libraryName);
            HttpResponseMessage responseMessage = httpClient.GetAsync(path).Result;
            string jsonStr = responseMessage.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<PackageVersion>(jsonStr);
        }

        // 获取包文件列表
        public Package GetFileList(ConfigPara para)
        {
            if(string.IsNullOrEmpty(para.Version))
            {
                PackageVersion version = GetLibraryVersions(para.Name);
                para.Version = version.Tag.Latest;
            }

            string path = Path.Combine(api, para.ToString());
            HttpResponseMessage responseMessage = httpClient.GetAsync(path).Result;
            string jsonStr = responseMessage.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<Package>(jsonStr);
        }

        public string Download(ConfigPara para)
        {
            Package packageFile = GetFileList(para);
            if(!Directory.Exists(para.Name))
                Directory.CreateDirectory(para.Name);
            
            SaveFile(para, string.Empty, packageFile.Files);
            return string.Empty;
        }

        private void SaveFile(ConfigPara para, string parentPath, List<PackageFile> packageFile)
        {
            packageFile.ForEach(file => 
            {
                string path = Path.Combine(parentPath, file.Name);
                if (file.Type == "directory")
                {
                    SaveFile(para, path, file.Files);
                }
                else
                {
                    string dirName = Path.Combine(para.Name, parentPath);
                    string localPath = Path.Combine(para.Name, path);
                    string remotePath = Path.Combine(url, para.ToString(), path);
                    
                    Console.WriteLine($"downloading {remotePath}");
                    HttpResponseMessage responseMessage = httpClient.GetAsync(remotePath).Result;
                    string content = responseMessage.Content.ReadAsStringAsync().Result;

                    Console.WriteLine($"writefile {localPath}");
                    if (!Directory.Exists(dirName))
                        Directory.CreateDirectory(dirName);

                    File.WriteAllText(localPath, content);
                }
            });
        }
    }
}