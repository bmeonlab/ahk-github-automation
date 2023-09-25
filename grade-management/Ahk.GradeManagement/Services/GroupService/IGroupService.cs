using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ahk.GradeManagement.Data.Entities;

namespace Ahk.GradeManagement.Services.GroupService
{
    public interface IGroupService
    {
        public Task SaveGroupAsync(string subjectId, Group group);
        public Task<List<Group>> ListGroupsAsync(string subject);
        public Task<List<Student>> ListStudentsAsync(string groupId);
        public Task<List<Teacher>> ListTeachersAsync(string groupId);
        public Task DeleteGroupAsync(int groupId);
        public Task UpdateGroupAsync(Group update);
        public Task AddStudentToGroupAsync(string subjectCode, string groupId, Student student);
        public Task AddTeacherToGroupAsync(string subjectCode, string groupId, Teacher teacher);
        public Task RemoveTeacherFromGroupAsync(string groupId, string teacherId);
        public Task RemoveStudentFromGroupAsync(string groupId, string studentId);
    }
}
