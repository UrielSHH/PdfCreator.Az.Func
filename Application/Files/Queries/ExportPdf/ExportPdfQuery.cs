using Application.Common.Interfaces;
using Domain.Dto.Response;
using MediatR;

namespace Application.Files.Queries.ExportPdf
{
    public class ExportPdfQuery : IRequestHandler<ExportPdfModel, FileResponseDto>
    {
        private readonly IPdfFileBuilder _pdfFileBuilder;

        public ExportPdfQuery(IPdfFileBuilder pdfFileBuilder)
        {
            _pdfFileBuilder = pdfFileBuilder;
        }

        public async Task<FileResponseDto> Handle(ExportPdfModel request, CancellationToken cancellationToken)
        {
            bool isValid = IsValidateRequest(request);

            FileResponseDto response = _pdfFileBuilder.Build(request.Template);

            return response;
        }

        private static bool IsValidateRequest(ExportPdfModel request)
        {
            var validator = new ExportPdfQueryValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                //save errors

                return false;
            }

            return true;
        }
    }
}
