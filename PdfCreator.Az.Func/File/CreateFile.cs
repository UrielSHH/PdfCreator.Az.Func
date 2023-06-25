using Application.Files.Queries.ExportPdf;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace PdfCreator.Az.Func.File
{
    public class CreateFile
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExportPdfModel> _logger;

        public CreateFile(IMediator mediator, ILogger<ExportPdfModel> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [FunctionName("CreatePdf")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            ExportPdfModel exportPdfModel = new ExportPdfModel();
            exportPdfModel.Template = data?.template;

            var res = await _mediator.Send(exportPdfModel);

            return new OkObjectResult(res);
        }
    }
}
