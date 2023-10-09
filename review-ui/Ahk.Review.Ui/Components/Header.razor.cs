using Ahk.Review.Ui.Models;
using Ahk.Review.Ui.Services;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

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
        [Inject]
        private ILocalStorageService LocalStorage { get; set; }

        private string subjectCode = string.Empty;
        private List<Subject> subjects = new List<Subject>();


        protected override async void OnInitialized()
        {
            var results = await Service.GetSubjects();
            subjects = results.ToList();

            Service.TenantCode = await LocalStorage.GetItemAsStringAsync("SelectedTenantCode");
            Service.CurrentTenant = JsonConvert.DeserializeObject<Subject>(await LocalStorage.GetItemAsStringAsync("SelectedTenant"));

            subjectCode = Service.TenantCode;

            StateHasChanged();
        }

        private async void SetTenant()
        {
            var subject = subjects.Where(s => s.SubjectCode == subjectCode).FirstOrDefault();
            Service.SetCurrentTenant(subjectCode, subject);
            await LocalStorage.SetItemAsStringAsync("SelectedTenantCode", subjectCode);
            await LocalStorage.SetItemAsStringAsync("SelectedTenant", JsonConvert.SerializeObject(subject));
        }

        private void Login()
        {
            NavigationManager.NavigateTo("authentication/login");
        }

        private void Logout()
        {
            NavigationManager.NavigateToLogout("/authentication/logout", "/");
        }
    }
}
