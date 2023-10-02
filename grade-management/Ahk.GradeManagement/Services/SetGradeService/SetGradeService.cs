using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ahk.GradeManagement.Data;
using Ahk.GradeManagement.Data.Entities;
using Ahk.GradeManagement.Services;
using Ahk.GradeManagement.SetGrade;

namespace Ahk.GradeManagement.Services.SetGradeService
{
    public class SetGradeService : ISetGradeService
    {
        private readonly IGradeService service;

        public SetGradeService(IGradeService service) => this.service = service;

        public async Task SetGradeAsync(SetGradeEvent data)
        {
            var previousResults = await service.GetLastResultOfAsync(neptun: Normalize.Neptun(data.Neptun), gitHubRepoName: Normalize.RepoName(data.Repository), gitHubPrNumber: data.PrNumber);

            await service.SetGradeAsync(data, previousResults);
        }

        public async Task ConfirmAutoGradeAsync(ConfirmAutoGradeEvent data)
        {
            var previousResults = await service.GetLastResultOfAsync(neptun: Normalize.Neptun(data.Neptun), gitHubRepoName: Normalize.RepoName(data.Repository), gitHubPrNumber: data.PrNumber);

            await service.UpdateGradeAsync(previousResults);
        }
    }
}
