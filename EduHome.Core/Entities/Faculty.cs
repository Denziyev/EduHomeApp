using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Faculty : BaseModel
    {
        [Required]
        public string Name { get; set; }
        List<Teacher>? Teachers { get; set; }
    }
}

