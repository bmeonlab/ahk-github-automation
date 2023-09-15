using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Ahk.GradeManagement.Data;
using Ahk.GradeManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Assignment>> ListAsync(string subject)
        {
            return Context.Assignments.Include(a => a.Subject).Where(a => a.Subject.SubjectCode == subject).ToList();
        }

        public async Task<List<Exercise>> ListExercisesAsync(string subject, string assignmentId)
        {
            return Context.Exercises.Include(e => e.Assignment)
                .ThenInclude(a => a.Subject)
                .Where(e => e.Assignment.Subject.SubjectCode == subject && e.AssignmentId.ToString() == assignmentId).ToList();
        }

        public async Task DeleteAssignmentAsync(string assignmentId)
        {
            Context.Remove(Context.Assignments.Where(a => a.Id.ToString() == assignmentId).FirstOrDefault());
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAssignmentAsync(Assignment assignment)
        {
            var assignmentToUpdate = await Context.Assignments.FindAsync(assignment.Id);
            assignmentToUpdate.Name = assignment.Name;
            assignmentToUpdate.DeadLine = assignment.DeadLine;
            assignmentToUpdate.ClassroomAssignment = assignment.ClassroomAssignment;

            await Context.SaveChangesAsync();
        }

        public async Task UpdateExercisesAsync(List<Exercise> exercises)
        {
            foreach (var exercise in exercises)
            {
                await UpdateExerciseAsync(exercise);
            }
            await Context.SaveChangesAsync();
        }

        private async Task UpdateExerciseAsync(Exercise exercise)
        {
            var exerciseToUpdate = await Context.Exercises.FindAsync(exercise.Id);
            exerciseToUpdate.Name = exercise.Name;
            exerciseToUpdate.AvailablePoints = exercise.AvailablePoints;
        }
    }
}
