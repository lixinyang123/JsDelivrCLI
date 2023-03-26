using JSDelivrCLI.Common;
using JSDelivrCLI.Models;
using System.Text.Json;

namespace JSDelivrCLI.Services
{
    public class CDNService
    {
        private readonly string api = "https://data.jsdelivr.com/v1/package/npm/";
        private readonly string url = "https://fastly.jsdelivr.net/npm/";
        private readonly string searchApi = "https://registry.npmjs.org/-/v1/search?text=";
        private readonly List<string> errorList;

        private readonly HttpClient httpClient;

        public CDNService()
        {
            httpClient = new HttpClient();
            errorList = new List<string>();
        }

        // 搜索包信息
        public async Task<SearchInfo> Search(string libraryName)
        {
            HttpResponseMessage responseMessage = await httpClient.GetAsync(searchApi + libraryName);
            string jsonStr = await responseMessage.Content.ReadAsStringAsync();
            SearchInfo searchInfo = JsonSerializer.Deserialize(jsonStr, SearchInfoJsonCtx.Default.SearchInfo);
            return searchInfo;
        }

        // 获取包版本信息
        public async Task<LibraryVersion> GetLibraryVersions(string libraryName)
        {
            string path = Path.Combine(api, libraryName);
            HttpResponseMessage responseMessage = await httpClient.GetAsync(path);
            string jsonStr = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize(jsonStr, LibraryVersionJsonCtx.Default.LibraryVersion);
        }

        // 获取包文件列表
        public async Task<Library> GetFileList(ConfigItem item)
        {
            LibraryVersion version = await GetLibraryVersions(item.Name);
            if (string.IsNullOrEmpty(item.Version))
            {
                ConsoleTool.WriteColorful($"Use latest version {version.Tag.Latest}\n", ConsoleColor.Green);
                item.Version = version.Tag.Latest;
            }

            if (!version.Versions.Contains(item.Version))
            {
                ConsoleTool.WriteColorful($"Can't find version {version.Tag.Latest}", ConsoleColor.Red);
                return null;
            }

            string path = Path.Combine(api, item.ToString());
            Console.WriteLine("Get library info...");

            HttpResponseMessage responseMessage = await httpClient.GetAsync(path);
            string jsonStr = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize(jsonStr, LibraryJsonCtx.Default.Library);
        }

        // 获取包清单并下载
        public async Task<bool> Download(ConfigItem item, string dir)
        {
            Library package = await GetFileList(item);

            if (package == null)
            {
                return false;
            }

            ConsoleTool.WriteColorful("Start downloading...\n", ConsoleColor.Blue);
            errorList.Clear();
            bool flag = SaveFile(dir, item, string.Empty, package.Files);

            errorList.ForEach(i =>
            {
                ConsoleTool.WriteColorful($"\nError: {i} download faled", ConsoleColor.Red);
            });
            return flag;
        }

        // 下载包
        private bool SaveFile(string saveDir, ConfigItem item, string parentPath, List<LibraryFile> packageFile)
        {
            List<Task> tasks = new();

            packageFile.ForEach(file =>
            {
                tasks.Add(Task.Run(() =>
                {
                    string path = Path.Combine(parentPath, file.Name);
                    if (file.Type == "directory")
                    {
                        SaveFile(saveDir, item, path, file.Files);
                    }
                    else
                    {
                        string dirName = Path.Combine(saveDir, item.Name, parentPath);
                        string localPath = Path.Combine(saveDir, item.Name, path);
                        string remotePath = Path.Combine(url, item.ToString(), path);

                        try
                        {
                            if (File.Exists(localPath))
                            {
                                return;
                            }

                            HttpResponseMessage responseMessage = httpClient.GetAsync(remotePath).Result;
                            string content = responseMessage.Content.ReadAsStringAsync().Result;

                            Console.WriteLine($"Writefile {localPath}");
                            if (!Directory.Exists(dirName))
                            {
                                Directory.CreateDirectory(dirName);
                            }

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