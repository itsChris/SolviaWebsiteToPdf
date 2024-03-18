using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolviaWebsiteToPdf
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string sitemapUrl = null;
            string outputFolder = null;
            string username = null;
            string password = null;

            // Parse command-line arguments
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-SiteMapUrl" && i + 1 < args.Length)
                {
                    sitemapUrl = args[++i];
                }
                else if (args[i] == "-OutputFolder" && i + 1 < args.Length)
                {
                    outputFolder = args[++i];
                }
                else if (args[i] == "-username" && i + 1 < args.Length)
                {
                    username = args[++i];
                }
                else if (args[i] == "-password" && i + 1 < args.Length)
                {
                    password = args[++i];
                }
            }

            // Check for mandatory parameters
            if (sitemapUrl == null || outputFolder == null)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  -SiteMapUrl <url> -OutputFolder <path> [-username <username> -password <password>]");
                Console.WriteLine("\n-SiteMapUrl and -OutputFolder are mandatory.");
                Console.WriteLine("-username and -password are optional for sites requiring authentication.");
                return;
            }

            var sitemapParser = new SitemapParser();
            var websiteToPdfConverter = new WebsiteToPdfConverter(outputFolder); // Assuming WebsiteToPdfConverter now accepts output folder as an argument

            List<string> urls;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                urls = await sitemapParser.ParseSitemapAsync(sitemapUrl, username, password);
            }
            else
            {
                urls = await sitemapParser.ParseSitemapAsync(sitemapUrl);
            }

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                await websiteToPdfConverter.GeneratePdfAsyncEx(urls, username, password);
            }
            else
            {
                await websiteToPdfConverter.GeneratePdfAsync(urls);
            }
            Console.WriteLine("PDF generation complete.");
        }
    }
}
