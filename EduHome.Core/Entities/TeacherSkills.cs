using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class TeacherSkills:BaseModel
    {
        public Teacher Teacher { get; set; }
        public int TeacherId { get; set; }
        public Skills Skills { get; set; }

        public int SkillsId { get; set;}

        public decimal Percent { get; set; }
    }
}
