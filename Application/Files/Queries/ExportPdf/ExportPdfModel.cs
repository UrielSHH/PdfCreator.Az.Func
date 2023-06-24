using Domain.Dto.Response;
using MediatR;

namespace Application.Files.Queries.ExportPdf
{
    public class ExportPdfModel : IRequest<FileResponseDto>
    {
        public string? Template { get; set; }
    }
}
