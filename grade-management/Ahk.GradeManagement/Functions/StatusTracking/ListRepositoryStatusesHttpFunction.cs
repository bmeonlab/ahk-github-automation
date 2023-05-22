using System.Threading.Tasks;
using Ahk.GradeManagement.Functions.StatusTracking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ahk.GradeManagement.StatusTracking
{
    public class ListRepositoryStatusesHttpFunction
    {
        private readonly IStatusTrackingService service;
        private readonly ILogger logger;

        public ListRepositoryStatusesHttpFunction(IStatusTrackingService service, ILoggerFactory loggerFactory)
        {
            this.service = service;
            this.logger = loggerFactory.CreateLogger<ListRepositoryStatusesHttpFunction>();
        }

        [Function("list-statuses")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Admin, "get", Route = "list-statuses/{*repoprefix}")] HttpRequest req,
            string repoprefix)
        {
            logger.LogInformation($"Received request to list statuses with prefix: {repoprefix}");

            var results = await service.ListStatusForRepositories(repoprefix);
            return new OkObjectResult(results);
        }
    }
}
