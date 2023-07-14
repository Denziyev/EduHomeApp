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
        [Required]
        public string Percent { get; set; }
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
    }
}
