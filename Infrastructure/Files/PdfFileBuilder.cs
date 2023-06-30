using Application.Common;
using Application.Common.Interfaces;
using Domain.Dto.Response;
using Microsoft.Extensions.Logging;
using SelectPdf;

namespace Infrastructure.Files
{
    public class PdfFileBuilder : IPdfFileBuilder
    {
        private readonly ILogger<PdfFileBuilder> _logger;

        public PdfFileBuilder(ILogger<PdfFileBuilder> logger)
        {
            _logger = logger;
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
                _logger.LogError($"An error ocurred while creating the file: {ex.Message}");
                fileResponseDto.Success = false;
                fileResponseDto.Errors.Add(string.Join(ConfigurationConstants.Pipe, ex.Message, ex.InnerException));
                return fileResponseDto;
            }
        }
    }
}
