using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahk.GradeManagement.Data.Entities
{
    public class TeacherSubject
    {
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }

        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
    }
}
