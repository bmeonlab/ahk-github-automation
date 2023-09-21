using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ahk.GradeManagement.Data.Entities;

namespace Ahk.GradeManagement.Services.SubjectService
{
    public interface ISubjectService
    {
        IReadOnlyCollection<Subject> GetAllSubjects();
        Task CreateSubjectAsync(Subject subject);
        Task EditSubjectAsync(Subject subject);
    }
}
