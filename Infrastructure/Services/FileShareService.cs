using Application.Common.Interfaces;
using Azure.Storage.Files.Shares;
using Domain.Dto.Response;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class FileShareService : IFileShareService
    {
        private readonly IConfiguration _configuration;

        public FileShareService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SaveFileAsync(FileResponseDto fileResponse)
        {
            try
            {
                string connectionString = _configuration["app.storage.connection.key1"];

                // Name of the share, directory, and file we'll create
                string shareName = "azfuncushfileshared";
                string dirName = "pdf-files";
                Guid fileNameGuid = Guid.NewGuid();
                string fileName = fileNameGuid.ToString() + ".pdf";

                // Get a reference to a share and then create it
                ShareClient share = new(connectionString, shareName);
                share.CreateIfNotExists();

                // Get a reference to a directory and create it
                ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
                directory.CreateIfNotExists();

                // Get bytes from file
                byte[]? fileBytes = Convert.FromBase64String(fileResponse.FileBase64);

                // Get a reference to a file and upload it
                ShareFileClient file = directory.CreateFile(fileName, fileBytes.Length);

                using MemoryStream stream = new(fileBytes);
                await file.UploadAsync(stream);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
