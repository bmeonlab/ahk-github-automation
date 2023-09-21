using System.Net;
using System.Threading.Tasks;

using Ahk.GradeManagement.Services.GroupService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Ahk.GradeManagement.Functions.Groups
{
    public class RemoveStudentFromGroupFunction
    {
        private readonly ILogger _logger;
        private readonly IGroupService groupService;

        public RemoveStudentFromGroupFunction(ILoggerFactory loggerFactory, IGroupService groupService)
        {
            _logger = loggerFactory.CreateLogger<RemoveStudentFromGroupFunction>();
            this.groupService = groupService;
        }

        [Function("remove-student")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "remove-student/{group}/{studentId}")] HttpRequestData req,
            string groupId, string studentId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            await groupService.RemoveStudentFromGroupAsync(groupId, studentId);

            return new OkResult();
        }
    }
}
