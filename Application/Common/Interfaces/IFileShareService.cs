using Domain.Dto.Response;

namespace Application.Common.Interfaces
{
    public interface IFileShareService
    {
        Task SaveFileAsync(FileResponseDto fileResponse);
    }
}
