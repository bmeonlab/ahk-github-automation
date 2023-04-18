using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahk.GradeManagement.Data.Entities
{
    public class StudentAssignment
    {
        public int StudentId { get; set; }
        public int AssignmentId { get; set; }

        public Student Student { get; set; }
        public Assignment Assignment { get; set; }
    }
}
