using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ahk.GradeManagement.Data;
using Ahk.GradeManagement.Data.Entities;

namespace Ahk.GradeManagement.Services.SubjectService
{
    public class SubjectService : ISubjectService
    {
        public AhkDbContext Context { get; set; }

        public SubjectService(AhkDbContext context) => this.Context = context;

        public IReadOnlyCollection<Subject> GetAllSubjects()
        {
            return Context.Subjects.ToList().AsReadOnly();
        }

        public async Task CreateSubjectAsync(Subject subject)
        {
            Context.Subjects.Add(subject);

            await Context.SaveChangesAsync();
        }

        public async Task EditSubjectAsync(Subject subject)
        {
            var oldSubject = Context.Subjects.Find(subject.Id);
            oldSubject.Name = subject.Name;
            oldSubject.SubjectCode = subject.SubjectCode;
            oldSubject.Semester = subject.Semester;
            oldSubject.GithubOrg = subject.GithubOrg;
            oldSubject.AhkConfig = subject.AhkConfig;

            await Context.SaveChangesAsync();
        }
    }
}
