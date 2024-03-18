using HtmlAgilityPack;
using System.Net.Http.Headers;

namespace SolviaWebsiteToPdf
{
    public class SitemapParser
    {
        private static string EncodeBasicAuthCredentials(string username, string password)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{username}:{password}");
            return Convert.ToBase64String(plainTextBytes);
        }
        public async Task<List<string>> ParseSitemapAsync(string sitemapUrl)
        {
            await Console.Out.WriteLineAsync("Will try to parse sitemap");
            var urls = new List<string>();
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(sitemapUrl);

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//url/loc"))
            {
                urls.Add(node.InnerText);
            }
            await Console.Out.WriteLineAsync($"According to the sitemap, there are {urls.Count} pages to process!");

            return urls;
        }

        public async Task<List<string>> ParseSitemapAsync(string sitemapUrl, string username, string password)
        {
            await Console.Out.WriteLineAsync("Will try to parse sitemap");
            var urls = new List<string>();

            using (var client = new HttpClient())
            {
                // Set the authorization header for Basic Auth
                var credentials = EncodeBasicAuthCredentials(username, password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                var response = await client.GetAsync(sitemapUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var doc = new HtmlDocument();
                    doc.LoadHtml(content);

                    foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//url/loc"))
                    {
                        urls.Add(node.InnerText);
                    }
                }
                else
                {
                    await Console.Out.WriteLineAsync($"Failed to download sitemap: {response.StatusCode}");
                    throw new Exception($"Failed to download sitemap: {response.StatusCode}");
                }
            }
            await Console.Out.WriteLineAsync($"According to the sitemap, there are {urls.Count} pages to process!");

            return urls;
        }
    }
}
