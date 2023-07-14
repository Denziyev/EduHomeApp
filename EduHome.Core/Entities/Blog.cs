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
    public class Blog:BaseModel
    {
        [Required]
        public string Title { get; set; }
        public DateTime? Time { get; set; }

        public string Writer { get; set; }

        public string? Image { get; set; }

        public string Content { get; set; }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public List<BlogTag>? BlogTags { get; set; }

        [NotMapped]
        public List<int>? TagIds { get; set; }

        [NotMapped]
        public IFormFile FormFile { get; set; }

    }
}
