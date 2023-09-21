using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Ahk.GradeManagement.Services.GroupService;


using AutoMapper;


using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Ahk.GradeManagement.Functions.Groups
{
    public class ListTeachersInGroupFunction
    {
        private readonly ILogger _logger;
        private readonly IGroupService groupService;
        private readonly Mapper mapper;

        public ListTeachersInGroupFunction(ILoggerFactory loggerFactory, IGroupService groupService, Mapper mapper)
        {
            _logger = loggerFactory.CreateLogger<ListTeachersInGroupFunction>();
            this.groupService = groupService;
            this.mapper = mapper;
        }

        [Function("list-teachers")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "list-teachers/{groupId}")] HttpRequestData req, string groupId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var teachers = mapper.Map<List<TeacherDTO>>(await groupService.ListTeachersAsync(groupId));

            return new OkObjectResult(teachers);
        }
    }
}
