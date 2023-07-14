using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class AboutWelcome:BaseModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description1 { get; set; }
        [Required]
        public string Description2 { get; set; }

        public string? Image { get; set; }
        public string Link { get; set; }

        [NotMapped]
        public IFormFile? FormFile { get; set; }

    }
}
