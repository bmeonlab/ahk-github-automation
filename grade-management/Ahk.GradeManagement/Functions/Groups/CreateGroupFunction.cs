using System;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Ahk.GradeManagement.Data.Entities;
using Ahk.GradeManagement.ResultProcessing.Dto;
using Ahk.GradeManagement.Services;
using Ahk.GradeManagement.Services.GroupService;
using Azure.Core;

using DTOs;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Ahk.GradeManagement.Functions.Groups
{
    public class CreateGroupFunction
    {
        private readonly ILogger logger;
        private readonly IGroupService service;

        public CreateGroupFunction(ILoggerFactory loggerFactory, IGroupService _service)
        {
            logger = loggerFactory.CreateLogger<CreateGroupFunction>();
            service = _service;
        }

        [Function("create-group")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "create-group/{subjectId}")] HttpRequestData request, string subjectId,
            [FromBody] GroupDTO groupDTO)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var group = new Group
            {
                Name = groupDTO.Name,
                Room = groupDTO.Room,
                Time = groupDTO.Time,
            };

            await service.SaveGroupAsync(subjectId, group);

            return new OkResult();
        }
    }
}
