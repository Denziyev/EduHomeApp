using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public  class Skills:BaseModel
    {
        [Required]
        public string Name { get; set; }
        public List<TeacherSkills> TeacherSkills { get; set; }
    }
}
