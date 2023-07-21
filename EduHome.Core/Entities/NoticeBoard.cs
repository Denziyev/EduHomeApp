using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class NoticeBoard:BaseModel
    {
        [Required]
        public string Content { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
