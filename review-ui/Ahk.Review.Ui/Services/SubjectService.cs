using Ahk.Review.Ui.Models;
using AutoMapper;
using DTOs;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;

using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Ahk.Review.Ui.Services
{
    public class SubjectService
    {
        private readonly HttpClient httpClient;
        public string TenantCode { get; set; }
        public Subject CurrentTenant { get; set; }

        public Mapper Mapper { get; set; }

        public event Action? OnChange;

        public SubjectService(IHttpClientFactory httpClientFactory, Mapper mapper)
        {
            this.httpClient = httpClientFactory.CreateClient("ApiClient");
            this.Mapper = mapper;
        }

        public async Task<IReadOnlyCollection<Subject>> GetSubjects()
        {
            var results = await httpClient.GetFromJsonAsync<OkObjectResult>($"list-subjects");

            var subjectDTOs = JsonConvert.DeserializeObject<List<SubjectDTO>>(results.Value.ToString());

            return subjectDTOs.Select(sDTO =>
            {
                return new Subject(sDTO);
            }).ToList();
        }

        public async Task CreateSubjectAsync(Subject subject)
        {
            await httpClient.PostAsJsonAsync<SubjectDTO>("create-subject", Mapper.Map<SubjectDTO>(subject));
        }

        public async Task EditSubjectAsync(Subject subject)
        {
            await httpClient.PostAsJsonAsync<SubjectDTO>("edit-subject", Mapper.Map<SubjectDTO>(subject));
        }

        public void SetCurrentTenant(string tenantCode, Subject subject)
        {
            TenantCode = tenantCode;
            CurrentTenant = subject;

            Console.WriteLine("SetCurrentTenant Called");

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
