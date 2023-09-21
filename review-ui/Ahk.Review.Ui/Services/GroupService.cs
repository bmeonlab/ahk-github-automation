using Ahk.Review.Ui.Models;
using AutoMapper;
using DTOs;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Ahk.Review.Ui.Services
{
    public class GroupService
    {
        private readonly HttpClient httpClient;

        private Mapper Mapper { get; set; }

        public GroupService(IHttpClientFactory httpClientFactory, Mapper mapper)
        {
            this.httpClient = httpClientFactory.CreateClient("ApiClient");
            this.Mapper = mapper;
        }

        public async Task CreateGroupAsync(string subjectId, Group group)
        {
            await httpClient.PostAsJsonAsync($"create-group/{subjectId}", Mapper.Map<GroupDTO>(group));
        }

        public async Task<List<Group>> GetGroupsAsync(string subject)
        {
            var response = await httpClient.GetFromJsonAsync<OkObjectResult>($"list-groups/{subject}");
            var groupDTOs = JsonConvert.DeserializeObject<List<GroupDTO>>(response.Value.ToString());

            return groupDTOs.Select(gDTO =>
            {
                return new Group(gDTO);
            }).ToList();
        }

        public async Task<Group> GetGroupAsync(string subject, string groupId)
        {
            var groups = await GetGroupsAsync(subject);
            var group = groups.Where(g => g.Id.ToString() == groupId).FirstOrDefault();

            return group;
        }

        public async Task UpdateGroupAsync(string subject, Group group)
        {
            await httpClient.PostAsJsonAsync<GroupDTO>($"edit-group/{subject}", Mapper.Map<GroupDTO>(group));
        }

        public async Task DeleteGroupAsync(string groupId)
        {
            await httpClient.DeleteAsync($"delete-group/{groupId}");
        }

        public async Task<List<Student>> ListStudentsInGroup(string groupId)
        {
            var response = await httpClient.GetFromJsonAsync<OkObjectResult>($"list-students/{groupId}");
            var studentDTOs = JsonConvert.DeserializeObject<List<StudentDTO>>(response.Value.ToString());

            return studentDTOs.Select(sDTO =>
            {
                return new Student(sDTO);
            }).ToList();
        }

        public async Task<List<Teacher>> ListTeachersInGroup(string groupId)
        {
            var response = await httpClient.GetFromJsonAsync<OkObjectResult>($"list-teachers/{groupId}");
            var teacherDTOs = JsonConvert.DeserializeObject<List<TeacherDTO>>(response.Value.ToString());

            return teacherDTOs.Select(tDTO =>
            {
                return new Teacher(tDTO);
            }).ToList();
        }

        public async Task RemoveTeacherFromGroup(string groupId, string teacherId)
        {
            await httpClient.DeleteAsync($"remove-teacher/{groupId}/{teacherId}");
        }

        public async Task RemoveStudentFromGroup(string groupId, string studentId)
        {
            await httpClient.DeleteAsync($"remove-student/{groupId}/{studentId}");
        }

        public async Task AddTeacherToGroup(string subject, string groupId, Teacher teacher)
        {
            await httpClient.PostAsJsonAsync<TeacherDTO>($"add-teacher/{subject}/{groupId}", Mapper.Map<TeacherDTO>(teacher));
        }

        public async Task AddStudentToGroup(string subject, string groupId, Student student)
        {
            await httpClient.PostAsJsonAsync<StudentDTO>($"add-student/{subject}/{groupId}", Mapper.Map<StudentDTO>(student));
        }
    }
}
