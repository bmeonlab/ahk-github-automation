using System.Net;
using System.Threading.Tasks;
using Ahk.GradeManagement.Services.AssignmentService;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Ahk.GradeManagement.Functions.Assignments
{
    public class DeleteAssignmentFunction
    {
        private readonly ILogger _logger;
        private readonly IAssignmentService assignmentService;

        public DeleteAssignmentFunction(ILoggerFactory loggerFactory, IAssignmentService assignmentService)
        {
            _logger = loggerFactory.CreateLogger<DeleteAssignmentFunction>();
            this.assignmentService = assignmentService;
        }

        [Function("delete-assignment")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "delete-assignment/{assignmentId}")] HttpRequestData req, string assignmentId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            await assignmentService.DeleteAssignmentAsync(assignmentId);

            return new OkResult();
        }
    }
}
