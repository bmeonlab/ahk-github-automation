using Ahk.Review.Ui.Models;
using Ahk.Review.Ui.Services;
using Microsoft.AspNetCore.Components;

namespace Ahk.Review.Ui.Pages.GroupPages
{
    public partial class CreateGroup : ComponentBase
    {
        [Inject]
        private GroupService GroupService { get; set; }
        [Inject]
        private SubjectService SubjectService { get; set; }

        private string name;
        private string room;
        private string time;

        protected override void OnInitialized()
        {
        }

        private async void Submit()
        {
            Group group = new Group()
            {
                Name = name,
                Room = room,
                Time = time,
            };

            await GroupService.CreateGroupAsync(SubjectService.CurrentTenant.Id.ToString(), group);
        }
    }
}
