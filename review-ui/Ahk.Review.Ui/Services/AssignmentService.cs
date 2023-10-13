using Ahk.Review.Ui.Models;
using AutoMapper;
using DTOs;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Ahk.Review.Ui.Services
{
    public class AssignmentService
    {
        private readonly HttpClient httpClient;

        public Mapper Mapper { get; set; }

        public AssignmentService(IHttpClientFactory httpClientFactory, Mapper mapper)
        {
            this.httpClient = httpClientFactory.CreateClient("ApiClient");
            this.Mapper = mapper;
        }

        public async Task CreateAssingmentAsync(string subject, Assignment assignment, List<Exercise> exercises)
        {
            var assignmentDTO = Mapper.Map<AssignmentDTO>(assignment);
            var exercisesDTO = Mapper.Map<List<ExerciseDTO>>(exercises);
            assignmentDTO.Exercises = exercisesDTO;

            subject = Uri.EscapeDataString(Uri.EscapeDataString(subject));

            await httpClient.PostAsJsonAsync($"create-assignment/{subject}", assignmentDTO);
        }

        public async Task<List<Assignment>> GetAssignmentsAsync(string subject)
        {
            subject = Uri.EscapeDataString(Uri.EscapeDataString(subject));

            var response = await httpClient.GetFromJsonAsync<OkObjectResult>($"list-assignments/{subject}");
            var assignmentDTOs = JsonConvert.DeserializeObject<List<AssignmentDTO>>(response.Value.ToString());

            return assignmentDTOs.Select(aDTO =>
            {
                return new Assignment(aDTO);
            }).ToList();
        }

        public async Task<Assignment> GetAssignmentAsync(string subject, string assignmentId)
        {
            var assignments = await GetAssignmentsAsync(subject);
            var assignment = assignments.Where(a => a.Id.ToString() == assignmentId).FirstOrDefault();

            return assignment;
        }

        public async Task<List<Exercise>> GetExercisesAsync(string subject, string assignmentId)
        {
            subject = Uri.EscapeDataString(Uri.EscapeDataString(subject));

            var response = await httpClient.GetFromJsonAsync<OkObjectResult>($"list-exercises/{subject}/{assignmentId}");
            var exerciseDTOs = JsonConvert.DeserializeObject<List<ExerciseDTO>>(response.Value.ToString());

            return exerciseDTOs.Select(eDTO =>
            {
                return new Exercise(eDTO);
            }).ToList();
        }

        public async Task EditAssignmentAsync(Assignment updateAssignment, List<Exercise> exercisesUpdate)
        {
            await httpClient.PostAsJsonAsync<AssignmentDTO>($"edit-assignment", Mapper.Map<AssignmentDTO>(updateAssignment));
            await httpClient.PostAsJsonAsync<List<ExerciseDTO>>($"edit-exercises", Mapper.Map<List<ExerciseDTO>>(exercisesUpdate));
        }

        public async Task DeleteAssignmentAsync(string assignmentId)
        {
            await httpClient.DeleteAsync($"delete-assignment/{assignmentId}");
        }
    }
}
