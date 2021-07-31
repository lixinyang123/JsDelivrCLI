using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using JSDelivrCLI.Models;

namespace JSDelivrCLI.Services
{
    public class CDNService
    {
        private string api = "https://data.jsdelivr.com/v1/package/npm/";

        private readonly HttpClient httpClient;

        public CDNService()
        {
            httpClient = new HttpClient();
        }

        // 获取包版本信息
        public async Task<PackageVersion> GetLibraryVersions(string libraryName)
        {
            HttpResponseMessage responseMessage = await httpClient.GetAsync($"{api}{libraryName}");
            string jsonStr = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PackageVersion>(jsonStr);
        }

        // 获取包文件列表
        public async Task<PackageFile> GetFileList(ConfigPara para)
        {
            if(string.IsNullOrEmpty(para.Version))
            {
                PackageVersion version = await GetLibraryVersions(para.Name);
                para.Version = version.Tag.Latest;
            }

            HttpResponseMessage responseMessage = await httpClient.GetAsync($"{api}{para.ToString()}");
            string jsonStr = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PackageFile>(jsonStr);
        }

        public async Task<string> Download(ConfigPara para)
        {
            PackageFile packageFile = await GetFileList(para);
            return JsonSerializer.Serialize(packageFile);
        }
    }
}