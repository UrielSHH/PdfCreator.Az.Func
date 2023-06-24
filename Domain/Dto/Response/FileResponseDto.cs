namespace Domain.Dto.Response
{
    public class FileResponseDto
    {
        public string? FileBase64 { get; set; }
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
