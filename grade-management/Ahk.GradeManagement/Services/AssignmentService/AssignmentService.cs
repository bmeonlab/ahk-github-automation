using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ahk.GradeManagement.Data;
using Ahk.GradeManagement.Data.Entities;

namespace Ahk.GradeManagement.Services.AssignmentService
{
    public class AssignmentService : IAssignmentService
    {
        private AhkDbContext Context { get; set; }

        public AssignmentService(AhkDbContext context)
        {
            Context = context;
        }

        public async Task SaveAssignmentAsync(Assignment assignment)
        {
            Context.Assignments.Add(assignment);
            await Context.SaveChangesAsync();
        }

        public async Task<List<Assignment>> ListAsync()
        {
            return Context.Assignments.ToList();
        }
    }
}