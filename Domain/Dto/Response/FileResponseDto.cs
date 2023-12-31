﻿namespace Domain.Dto.Response
{
    public class FileResponseDto
    {
        public string? FileBase64 { get; set; }
        public string? FileName { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
