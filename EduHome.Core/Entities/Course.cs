using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Course:BaseModel
    {
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public string? Image { get; set; }

        public string Description { get; set; }
        public string Abouttext { get; set; }
        public string Applytext { get; set; }
        public string Certification { get; set; }

        public Feature? Feature { get; set; }
        public int FeatureId { get; set; }

        public List<CourseTag>? CourseTags { get; set; }

        [NotMapped]
        public List<int>? TagIds { get; set; }

        [NotMapped]
        public IFormFile? FormFile { get; set; }
    }
}
