# Website to PDF Converter

This program automates the process of generating PDFs from web pages listed in a given sitemap.xml. It supports basic HTTP authentication for accessing protected sitemaps or web pages. The converter is designed to handle dynamic content by waiting for network idle states and additional timeouts to ensure complete page loading.

## Features

- Parse `sitemap.xml` to extract website URLs.
- Generate PDFs from web pages with options for handling dynamic content.
- Support for HTTP Basic Authentication to access protected resources.
- Customizable output directory for storing generated PDFs.
- Command-line argument support for flexible operation.

## Requirements

- .NET Core 3.1 or higher.
- PuppeteerSharp package (for headless browser support).
- HtmlAgilityPack package (for parsing sitemap.xml).

## Installation

1. Ensure .NET Core SDK is installed on your machine.
2. Clone the repository to your local machine.
3. Navigate to the project directory and restore the required NuGet packages:

```bash
dotnet restore
```

4. Build the project:

```bash
dotnet build
```

## Usage

Run the program from the command line, specifying the required parameters:

```bash
SolviaWebsiteToPdf.exe -SiteMapUrl <SitemapURL> -OutputFolder <PathToOutputFolder> [-username <Username> -password <Password>]
```

- `-SiteMapUrl`: The URL or path to the sitemap.xml file.
- `-OutputFolder`: The path to the directory where PDFs will be saved.
- `-username` (Optional): Username for HTTP Basic Authentication.
- `-password` (Optional): Password for HTTP Basic Authentication.

### Example

```bash
SolviaWebsiteToPdf.exe -SiteMapUrl https://www.solvia.ch/page-sitemap.xml -OutputFolder C:\PDFs
```

## Customization

You can customize the behavior of the PDF generation (e.g., changing the timeout or the PDF format) by modifying the `WebsiteToPdfConverter.cs` file.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue for bugs, feature requests, or documentation improvements.

## License

This project is open-sourced under the MIT License. See the LICENSE file for more details.

