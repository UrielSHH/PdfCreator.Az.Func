using Application.Common;
using Application.Common.Interfaces;
using Azure.Storage.Files.Shares;
using Domain.Dto.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class FileShareService : IFileShareService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileShareService> _logger;

        public FileShareService(
            IConfiguration configuration,
            ILogger<FileShareService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SaveFileAsync(FileResponseDto fileResponse)
        {
            try
            {
                string connectionString = _configuration[ConfigurationConstants.FileShareConnection];

                // Name of the share, directory and file name
                string shareName = _configuration[ConfigurationConstants.FileShareName];
                string dirName = _configuration[ConfigurationConstants.DirectoryName];
                fileResponse.FileName = GetFileName();
                // Get a reference to a share and then create it if not exists
                ShareClient share = new(connectionString, shareName);
                share.CreateIfNotExists();

                // Get a reference to a directory and create it if not exists
                ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
                directory.CreateIfNotExists();

                // Get bytes from file
                byte[]? fileBytes = Convert.FromBase64String(fileResponse.FileBase64);

                // Get a reference to a file and upload it
                ShareFileClient file = directory.CreateFile(fileResponse.FileName, fileBytes.Length);

                using MemoryStream stream = new(fileBytes);
                var response = await file.UploadAsync(stream);
                if (response.GetRawResponse().IsError)
                {
                    string error = "An error occurred while uploading the file to Azure File Share";
                    _logger.LogError(error);
                    fileResponse.Success = false;
                    fileResponse.Errors.Add(error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Join(ConfigurationConstants.Pipe, $"Thrown exception while uploading the file to Azure File Share", ex.Message, ex.InnerException));
                throw;
            }
        }

        private string GetFileName()
        {
            try
            {
                Guid fileNameGuid = Guid.NewGuid();
                string fileName = fileNameGuid.ToString() + ConfigurationConstants.PdfExtension;
                return fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating the file name: {ex.Message}");
                throw;
            }
        }
    }
}
