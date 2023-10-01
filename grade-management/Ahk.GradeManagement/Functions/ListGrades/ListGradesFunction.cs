using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ahk.GradeManagement.Functions.Groups;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Ahk.GradeManagement.ListGrades
{
    public class ListGradesFunction
    {
        private readonly IGradeListing service;
        private readonly ILogger logger;

        public ListGradesFunction(IGradeListing service, ILoggerFactory loggerFactory)
        {
            this.service = service;
            this.logger = loggerFactory.CreateLogger<ListGradesFunction>();
        }

        [Function("list-grades")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "list-grades/{subject}/{*repoprefix}")] HttpRequestData req, string subject,
            string repoprefix)
        {
            logger.LogInformation($"Received request to list grades with prefix: {repoprefix}");

            var acceptHeader = req.Headers.GetValues(HeaderNames.Accept).First();
            subject = Uri.UnescapeDataString(subject);
            var results = await service.List(subject, repoprefix);

            if (acceptHeader.Equals("text/csv", StringComparison.OrdinalIgnoreCase))
                return new FileContentResult(Encoding.UTF8.GetBytes(CsvExporter.GetCsv(results)), "text/csv; charset=utf-8");
            else
                return new OkObjectResult(results);
        }
    }
}
