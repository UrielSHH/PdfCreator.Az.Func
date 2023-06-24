using Application.Common.Interfaces;
using Domain.Dto.Response;
using Microsoft.Extensions.Configuration;
using SelectPdf;

namespace Infrastructure.Files
{
    public class PdfFileBuilder : IPdfFileBuilder
    {
        private readonly IConfiguration _configuration;

        public PdfFileBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public FileResponseDto Build(string template)
        {
            FileResponseDto fileResponseDto = new();
            try
            {
                // instantiate a html to pdf converter object
                HtmlToPdf converter = new();

                // set converter options
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                //converter.Options.WebPageWidth = webPageWidth;
                //converter.Options.WebPageHeight = webPageHeight;

                // create a new pdf document converting an url
                PdfDocument doc = converter.ConvertHtmlString(template);

                // save pdf document
                var docBytes = doc.Save();
                // close pdf document
                doc.Close();

                fileResponseDto.FileBase64 = Convert.ToBase64String(docBytes);
                fileResponseDto.Success = true;

                return fileResponseDto;
            }
            catch (Exception ex)
            {
                fileResponseDto.Success = false;
                fileResponseDto.Error = string.Join("|", ex.Message, ex.InnerException);
                return fileResponseDto;
            }
        }
    }
}
