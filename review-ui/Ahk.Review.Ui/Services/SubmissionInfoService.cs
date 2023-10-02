using System.Net.Http.Json;
using Ahk.Review.Ui.Models;
using AutoMapper;
using DTOs;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace Ahk.Review.Ui.Services
{
    public class SubmissionInfoService
    {
        private readonly HttpClient httpClient;

        public Mapper Mapper { get; set; }

        public SubmissionInfoService(IHttpClientFactory httpClientFactory, Mapper mapper)
        {
            this.httpClient = httpClientFactory.CreateClient("ApiClient");
            this.Mapper = mapper;
        }

        public async Task<List<SubmissionInfo>> GetData(string subject, string repositoryPrefix)
        {
            var repoStatResponse = await httpClient.GetFromJsonAsync<OkObjectResult>($"list-statuses/{repositoryPrefix}");
            var gradesResponse = await httpClient.GetFromJsonAsync<OkObjectResult>($"list-grades/{Uri.EscapeDataString(subject)}/{repositoryPrefix}");
            var eventsResponse = await httpClient.GetFromJsonAsync<OkObjectResult>($"list-events/{repositoryPrefix}");

            var repoStat = JsonConvert.DeserializeObject<List<SubmissionInfoDTO>>(repoStatResponse.Value.ToString());
            var grades = JsonConvert.DeserializeObject<List<FinalStudentGrade>>(gradesResponse.Value.ToString());
            var events = JsonConvert.DeserializeObject<List<StatusEventBaseDTO>>(eventsResponse.Value.ToString());

            return mergeResults(repoStat!, grades!, events!);
        }

        public async Task<Stream> DownloadGradesCsv(string subject, string repositoryPrefix)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, $"list-grades/{Uri.EscapeDataString(subject)}/{repositoryPrefix}");
            req.Headers.Remove("Accept");
            req.Headers.Add("Accept", "text/csv");
            var resp = await httpClient.SendAsync(req);
            resp.EnsureSuccessStatusCode();

            return await resp.Content.ReadAsStreamAsync();
        }

        private static List<SubmissionInfo> mergeResults(List<SubmissionInfoDTO> submissionInfoDTOs, List<FinalStudentGrade> grades, List<StatusEventBaseDTO> events)
        {
            if (submissionInfoDTOs.FirstOrDefault() == null || grades.FirstOrDefault() == null || events.FirstOrDefault() == null)
            {
                return new List<SubmissionInfo>();
            }

            var gradesLookup = grades.ToDictionary(g => g.Repo);
            return submissionInfoDTOs.Select(r =>
            {
                gradesLookup.TryGetValue(r.Repository, out var g);
                return new SubmissionInfo(g.AssignmentName, r.Repository, r.Neptun, r.Branches, r.PullRequests, r.WorkflowRuns, g?.Points, events, false);
            }).ToList();
        }
    }
}
