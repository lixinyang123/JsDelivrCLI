using System.Net.Http;

namespace JSDelivrCLI.Services
{
    public static class JsDelivrService
    {
        private static string api = "https://data.jsdelivr.com/v1/package/npm/";

        public static void GetPackagesList()
        {
            HttpClient httpClient = new HttpClient();

        }
    }
}