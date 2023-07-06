using Application.Common.Interfaces;
using Domain.Dto.Response;
using FluentValidation.Results;
using MediatR;

namespace Application.Files.Queries.ExportPdf
{
    public class ExportPdfQuery : IRequestHandler<ExportPdfModel, FileResponseDto>
    {
        private readonly IPdfFileBuilder _pdfFileBuilder;
        private readonly IFileShareService _fileShareService;

        public ExportPdfQuery(IPdfFileBuilder pdfFileBuilder, IFileShareService fileShareService)
        {
            _pdfFileBuilder = pdfFileBuilder;
            _fileShareService = fileShareService;
        }

        public async Task<FileResponseDto> Handle(ExportPdfModel request, CancellationToken cancellationToken)
        {
            //Check for errors in request
            IReadOnlyList<ValidationFailure>? errors = IsValidateRequest(request);
            if (errors != null)
                return new FileResponseDto { Success = false, Errors = errors.Select(x => x.ErrorMessage).ToList() };

            //Create Pdf file 
            FileResponseDto response = _pdfFileBuilder.Build(request.Template);
            if (!response.Success)
                return response;

            //Send to file share
            await _fileShareService.SaveFileAsync(response);

            return response;
        }

        private static IReadOnlyList<ValidationFailure>? IsValidateRequest(ExportPdfModel request)
        {
            var validator = new ExportPdfQueryValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return validationResult.Errors;
            }

            return null;
        }
    }
}
