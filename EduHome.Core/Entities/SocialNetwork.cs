using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class SocialNetwork : BaseModel
    {
        public string Icon { get; set; }
        public string Link { get; set; }

        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

    }
}
