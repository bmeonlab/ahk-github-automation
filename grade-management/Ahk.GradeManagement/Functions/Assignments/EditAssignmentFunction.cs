using System.Net;
using System.Threading.Tasks;

using Ahk.GradeManagement.Data.Entities;
using Ahk.GradeManagement.Services.AssignmentService;
using Ahk.GradeManagement.Services.GroupService;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Ahk.GradeManagement.Functions.Assignments
{
    public class EditAssignmentFunction
    {
        private readonly ILogger _logger;
        private readonly IAssignmentService assignmentService;

        public EditAssignmentFunction(ILoggerFactory loggerFactory, IAssignmentService assignmentService)
        {
            _logger = loggerFactory.CreateLogger<EditAssignmentFunction>();
            this.assignmentService = assignmentService;
        }

        [Function("edit-assignment")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "edit-assignment")] HttpRequestData req,
            [FromBody] AssignmentDTO assignmentDTO)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var update = new Assignment()
            {
                Id = assignmentDTO.Id,
                Name = assignmentDTO.Name,
                DeadLine = assignmentDTO.DeadLine,
                ClassroomAssignment = assignmentDTO.ClassroomAssignment,
                SubjectId = assignmentDTO.SubjectId,
            };

            await assignmentService.UpdateAssignmentAsync(update);

            return new OkResult();
        }
    }
}
