using Ahk.Review.Ui.Models;
using Ahk.Review.Ui.Services;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace Ahk.Review.Ui.Components
{
    public partial class Header : ComponentBase
    {
        [Inject]
        private SubjectService Service { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private string subjectCode = string.Empty;
        private List<Subject> subjects = new List<Subject>();


        protected override async void OnInitialized()
        {
            var results = await Service.GetSubjects();
            subjects = results.ToList();
        }

        private void SetTenant()
        {
            var subject = subjects.Where(s => s.SubjectCode == subjectCode).FirstOrDefault();
            Service.SetCurrentTenant(subjectCode, subject);
            JSRuntime.InvokeAsync<string>("localStorage.setItem", "SelectedTenantCode", subjectCode);
            JSRuntime.InvokeAsync<string>("localStorage.setItem", "SelectedTenant", subject);

        }
    }
}
