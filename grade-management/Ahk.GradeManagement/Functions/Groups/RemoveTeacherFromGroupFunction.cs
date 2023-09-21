using System.Net;
using System.Threading.Tasks;

using Ahk.GradeManagement.Services.GroupService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Ahk.GradeManagement.Functions.Groups
{
    public class RemoveTeacherFromGroupFunction
    {
        private readonly ILogger _logger;
        private readonly IGroupService groupService;

        public RemoveTeacherFromGroupFunction(ILoggerFactory loggerFactory, IGroupService groupService)
        {
            _logger = loggerFactory.CreateLogger<RemoveTeacherFromGroupFunction>();
            this.groupService = groupService;
        }

        [Function("remove-teacher")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "remove-teacher/{groupId}/{teacherId}")] HttpRequestData req,
            string groupId, string teacherId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            await groupService.RemoveTeacherFromGroupAsync(groupId, teacherId);

            return new OkResult();
        }
    }
}