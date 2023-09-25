using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Ahk.GradeManagement.Data;
using Ahk.GradeManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ahk.GradeManagement.Services.GroupService
{
    public class GroupService : IGroupService
    {
        private AhkDbContext Context { get; set; }

        public GroupService(AhkDbContext context)
        {
            Context = context;
        }

        public async Task SaveGroupAsync(string subjectId, Group group)
        {
            var subject = Context.Subjects.Find(Int32.Parse(subjectId));

            group.Subject = subject;

            Context.Groups.Add(group);
            await Context.SaveChangesAsync();
        }

        public async Task<List<Group>> ListGroupsAsync(string subject)
        {
            return Context.Groups.Include(g => g.Subject).Where(g => g.Subject.SubjectCode == subject).ToList();
        }

        public async Task<List<Student>> ListStudentsAsync(string groupId)
        {
            var studentGroups = Context.StudentGroups.Include(g => g.Student).Where(g => g.GroupId.ToString() == groupId).ToList();
            var students = new List<Student>();
            foreach (var studentGroup in studentGroups)
            {
                var student = studentGroup.Student;
                students.Add(student);
            }
            return students;
        }

        public async Task<List<Teacher>> ListTeachersAsync(string groupId)
        {
            var teachersGroups = Context.TeacherGroups.Where(g => g.GroupId.ToString() == groupId).ToList();
            var teachers = new List<Teacher>();
            foreach (var teacherGroup in teachersGroups)
            {
                var teacher = teacherGroup.Teacher;
                teachers.Add(teacher);
            }
            return teachers;
        }

        public async Task DeleteGroupAsync(int groupId)
        {
            var group = Context.Groups.Find(groupId);
            Context.Remove(group);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateGroupAsync(Group update)
        {
            var groupToUpdate = await Context.Groups.FindAsync(update.Id);
            groupToUpdate.Name = update.Name;
            groupToUpdate.Room = update.Room;
            groupToUpdate.Time = update.Time;

            await Context.SaveChangesAsync();
        }

        public async Task AddStudentToGroupAsync(string subjectCode, string groupId, Student student)
        {
            StudentGroup studentGroup = new StudentGroup
            {
                Student = student,
                Group = await Context.Groups.FindAsync(Int32.Parse(groupId)),
            };

            Subject subject = Context.Subjects.Where(s => s.SubjectCode == subjectCode).FirstOrDefault();

            StudentSubject studentSubject = new StudentSubject
            {
                Student = student,
                Subject = subject,
            };

            if (!Context.StudentSubjects.Where(ss => ss.Subject.SubjectCode == subject.SubjectCode && ss.Student.Neptun.ToLower() == student.Neptun.ToLower()).Any())
            {
                Context.StudentSubjects.Add(studentSubject);
            }

            Context.StudentGroups.Add(studentGroup);

            if (!Context.Students.Where(s => s.Neptun.ToLower() == student.Neptun.ToLower()).Any())
            {
                Context.Students.Add(student);
            }

            await Context.SaveChangesAsync();
        }

        public async Task AddTeacherToGroupAsync(string subjectCode, string groupId, Teacher teacher)
        {
            TeacherGroup teacherGroup = new TeacherGroup
            {
                Teacher = teacher,
                Group = Context.Groups.Find(Int32.Parse(groupId)),
            };

            Subject subject = Context.Subjects.Where(s => s.SubjectCode == subjectCode).FirstOrDefault();

            TeacherSubject teacherSubject = new TeacherSubject
            {
                Teacher = teacher,
                Subject = subject,
            };

            if (!Context.TeacherSubjects.Where(ts => ts.Subject.SubjectCode == subject.SubjectCode && ts.Teacher.Neptun.ToLower() == teacher.Neptun.ToLower()).Any())
            {
                Context.TeacherSubjects.Add(teacherSubject);
            }

            Context.TeacherGroups.Add(teacherGroup);

            if (!Context.Teachers.Where(t => t.Neptun.ToLower() == teacher.Neptun.ToLower()).Any())
            {
                Context.Teachers.Add(teacher);
            }

            await Context.SaveChangesAsync();
        }

        public async Task RemoveTeacherFromGroupAsync(string groupId, string teacherId)
        {
            Context.TeacherGroups.Remove(Context.TeacherGroups.Where(tg => tg.GroupId.ToString() == groupId && tg.TeacherId.ToString() == teacherId).FirstOrDefault());

            await Context.SaveChangesAsync();
        }

        public async Task RemoveStudentFromGroupAsync(string groupId, string studentId)
        {
            Context.StudentGroups.Remove(Context.StudentGroups.Where(sg => sg.GroupId.ToString() == groupId && sg.StudentId.ToString() == studentId).FirstOrDefault());

            await Context.SaveChangesAsync();
        }
    }
}
