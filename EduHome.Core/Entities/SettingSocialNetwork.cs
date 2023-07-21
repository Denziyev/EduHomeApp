using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class SettingSocialNetwork:BaseModel
    {
        public string Icon { get; set; }
        public string Link { get; set; }

        public int SettingId { get; set; }
        public Setting? Setting { get; set; }
    }
}
