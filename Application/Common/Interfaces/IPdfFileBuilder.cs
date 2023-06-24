using Domain.Dto.Response;

namespace Application.Common.Interfaces
{
    public interface IPdfFileBuilder
    {
        FileResponseDto Build(string template);
    }
}
