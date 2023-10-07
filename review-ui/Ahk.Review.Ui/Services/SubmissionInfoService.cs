using System.Net.Http.Json;
using Ahk.Review.Ui.Models;
using AutoMapper;
using DTOs;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            subject = Uri.EscapeDataString(Uri.EscapeDataString(subject));

            var repoStatResponse = await httpClient.GetFromJsonAsync<OkObjectResult>($"list-statuses/{repositoryPrefix}");
            var gradesResponse = await httpClient.GetFromJsonAsync<OkObjectResult>($"list-grades/{subject}/{repositoryPrefix}");
            var eventsResponse = await httpClient.GetFromJsonAsync<OkObjectResult>($"list-events/{repositoryPrefix}");

            var repoStat = JsonConvert.DeserializeObject<List<SubmissionInfoDTO>>(repoStatResponse.Value.ToString());
            var grades = JsonConvert.DeserializeObject<List<FinalStudentGrade>>(gradesResponse.Value.ToString());
            var events = JsonConvert.DeserializeObject<List<StatusEventBaseDTO>>(eventsResponse.Value.ToString());

            return mergeResults(repoStat!, grades!, events!);
        }

        public async Task<Stream> DownloadGradesCsv(string subject, string repositoryPrefix)
        {
            subject = Uri.EscapeDataString(Uri.EscapeDataString(subject));

            using var req = new HttpRequestMessage(HttpMethod.Get, $"list-grades/{subject}/{repositoryPrefix}");
            req.Headers.Remove("Accept");
            req.Headers.Add("Accept", "text/csv");
            var resp = await httpClient.SendAsync(req);
            resp.EnsureSuccessStatusCode();

            var result = await resp.Content.ReadAsStringAsync();

            JObject jsonObject = JObject.Parse(result);

            string base64FileContent = (string)jsonObject["FileContents"];
            byte[] fileContent = Convert.FromBase64String(base64FileContent);

            Console.WriteLine(base64FileContent);

            return new MemoryStream(fileContent);
        }

        private static List<SubmissionInfo> mergeResults(List<SubmissionInfoDTO> submissionInfoDTOs, List<FinalStudentGrade> grades, List<StatusEventBaseDTO> events)
        {
            if (!submissionInfoDTOs.Any() || !grades.Any() || !events.Any())
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
