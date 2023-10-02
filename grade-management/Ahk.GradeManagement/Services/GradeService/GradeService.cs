using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ahk.GradeManagement.Data;
using Ahk.GradeManagement.Data.Entities;
using Ahk.GradeManagement.ResultProcessing.Dto;

using Microsoft.EntityFrameworkCore;

namespace Ahk.GradeManagement.Services
{
    public class GradeService : IGradeService
    {
        public AhkDbContext Context { get; set; }
        public GradeService(AhkDbContext context)
        {
            Context = context;
        }

        public async Task AddGradeAsync(Grade value)
        {
            Context.Grades.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateGradeAsync(Grade grade)
        {
            var oldGrade = Context.Grades.Find(grade.Id);
            oldGrade.IsConfirmed = true;
            oldGrade.Date = DateTimeOffset.UtcNow;

            await Context.SaveChangesAsync();
        }

        //public async Task SetGradeAsync()

        public async Task<Grade> GetLastResultOfAsync(string neptun, string gitHubRepoName, int gitHubPrNumber)
        {
            var grades = Context.Grades.Include(g => g.Student).Include(g => g.Assignment)
                .Where(s => s.Student.Neptun == neptun.ToUpperInvariant() && s.GithubRepoName == gitHubRepoName.ToLowerInvariant() && s.GithubPrNumber == gitHubPrNumber)
                .OrderByDescending(s => s.Date);

            return grades.FirstOrDefault();
        }
        public async Task<IReadOnlyCollection<Grade>> ListConfirmedWithRepositoryPrefixAsync(string subject, string repoPrefix)
        {
            var confirmedGrades = Context.Grades.Include(g => g.Student).Include(g => g.Assignment).ThenInclude(a => a.Subject).Include(g => g.Points).ThenInclude(p => p.Exercise)
                .Where(s => s.IsConfirmed && s.GithubRepoName.StartsWith(repoPrefix) && s.Assignment.Subject.SubjectCode == subject);
            return confirmedGrades.ToList().AsReadOnly();
        }

        public async Task DeleteGrade(int id)
        {
            var grade = Context.Grades.Find(id);

            Context.Grades.Remove(grade);
            await Context.SaveChangesAsync();
        }

        public Student FindStudent(string neptun)
        {
            return Context.Students.Where(s => s.Neptun == neptun).FirstOrDefault();
        }

        public Assignment FindAssignment(AhkTaskResult[] results)
        {
            string firstExercise = results[0].ExerciseName;
            string firstTask = results[0].TaskName;
            var assignment = Context.Assignments.Include(a => a.Exercises).Where(a => a.Exercises.First().Name == firstExercise || a.Exercises.First().Name == firstTask).FirstOrDefault();

            return assignment;
        }

        public int FindAssignmentId(AhkTaskResult[] results)
        {
            return FindAssignment(results).Id;
        }

        public List<Point> GetPoints(AhkTaskResult[] results)
        {
            var points = new List<Point>();

            foreach (var result in results)
            {
                var point = new Point()
                {
                    Exercise = Context.Exercises.Where(e => e.Name == result.ExerciseName || e.Name == result.TaskName).FirstOrDefault(),
                    PointEarned = result.Points,
                };

                points.Add(point);
            }

            return points;
        }

    }
}
