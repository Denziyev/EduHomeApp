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
    public class Teacher:BaseModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Description { get; set; }
        public string? Image { get; set; }

        public string Degree { get; set; }

        public int Experience { get; set; }

        public string? Hobbies { get;set; }

        public string Mail { get; set; }

        public string Phone { get; set; }

        public string Skype { get; set; }


        [ForeignKey("Position")]
        public int PositionId { get; set; }
        public Position? Position { get; set; }

        public int FacultyId { get; set; }
        public Faculty? Faculty { get; set; }

        public List<SocialNetwork>? SocialNetworks { get; set; }
        public List<Skills>? Skills { get; set; }

        [NotMapped]
        public IFormFile? FormFile { get; set; }

    }
}
