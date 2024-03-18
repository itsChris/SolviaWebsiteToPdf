using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace SolviaWebsiteToPdf
{
    public class WebsiteToPdfConverter
    {
        private string SanitizeUrl(string url)
        {
            // List of characters that are not allowed in file names in Windows.
            var invalidChars = Path.GetInvalidFileNameChars();
            // Replace invalid characters with an underscore.
            var sanitized = new string(url.Select(c => invalidChars.Contains(c) ? '_' : c).ToArray());
            // Optionally, further transform the sanitized string here (e.g., shorten it if too long).
            return sanitized;
        }

        public async Task GeneratePdfAsync(List<string> urls)
        {
            // faster, but does not wait for network idle
            await new BrowserFetcher().DownloadAsync();
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

            foreach (var url in urls)
            {
                var page = await browser.NewPageAsync();
                await page.GoToAsync(url);
                var sanitizedUrl = SanitizeUrl(url);
                var fileName = $"{sanitizedUrl}.pdf";
                await page.PdfAsync(fileName);
            }

            await browser.CloseAsync();
        }

        public async Task GeneratePdfAsyncEx(List<string> urls)
        {
            await new BrowserFetcher().DownloadAsync();
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

            foreach (var url in urls)
            {
                var page = await browser.NewPageAsync();
                await page.GoToAsync(url, new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }, Timeout = 60000 });
                // Additional waits or JavaScript execution here if necessary
                var sanitizedUrl = SanitizeUrl(url);
                var fileName = $"{sanitizedUrl}.pdf";
                await page.PdfAsync(fileName, new PdfOptions { Format = PaperFormat.A4, PrintBackground = true });
            }

            await browser.CloseAsync();
        }

        public async Task GeneratePdfAsyncEx(List<string> urls, string username, string password)
        {
            await new BrowserFetcher().DownloadAsync();
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

            foreach (var url in urls)
            {
                Console.WriteLine($"Processing {url}");
                var page = await browser.NewPageAsync();
                // set viewport to 1920x1080
                await page.SetViewportAsync(new ViewPortOptions { Width = 1920, Height = 1080 });
                
                // set user agent (override the default headless User Agent)
                await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

                // Encode the credentials and set the Authorization header
                var encodedCredentials = EncodeBasicAuthCredentials(username, password);
                await page.SetExtraHttpHeadersAsync(new Dictionary<string, string>
                    {
                        { "Authorization", $"Basic {encodedCredentials}" }
                    });

                await page.GoToAsync(url, new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }, Timeout = 60000 });
                // Additional waits or JavaScript execution here if necessary

                // wait an additional 5 seconds to ensure all content is loaded
                Console.WriteLine("Waiting for 5 seconds to ensure all content is loaded");
                await page.WaitForTimeoutAsync(5000);

                var sanitizedUrl = SanitizeUrl(url);
                var fileName = $"{sanitizedUrl}.pdf";
                Console.WriteLine($"saving to {fileName}");
                await page.PdfAsync(fileName, new PdfOptions { Format = PaperFormat.A4, Landscape=true, PrintBackground = true });
            }

            await browser.CloseAsync();
        }
        private string EncodeBasicAuthCredentials(string username, string password)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{username}:{password}");
            return Convert.ToBase64String(plainTextBytes);
        }

    }
}