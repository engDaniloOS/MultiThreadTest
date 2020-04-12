using System;
using System.Net;
using System.Threading.Tasks;

namespace ThreadTest
{
    public class ConsultaWeb
    {
        public async Task<bool> AcessoAsync(string url)
        {
            HttpWebRequest webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = "GET";

            try
            {
                HttpWebResponse response = (HttpWebResponse)await webRequest.GetResponseAsync();

                return response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"It wasn't possible to execute the action. Error: {ex.Message}");
                return false;
            }
        }

        public bool Acesso(string url)
        {
            HttpWebRequest webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = "GET";

            try
            {
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                return response.StatusCode == HttpStatusCode.OK ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"It wasn't possible to execute the action. Error: {ex.Message}");
                return false;
            }
        }
    }
}
