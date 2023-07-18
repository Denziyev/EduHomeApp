using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Setting:BaseModel
    {
        public string Logo { get; set; }
        [NotMapped]
        public IFormFile? LogoFormFile { get; set; }

        public string NavbarTitle { get; set; }
        public string MainPhone { get; set; }

        public string DescriptionFooter { get; set; }

        public List<SocialNetwork>? SocialNetworks { get; set; }
        public string CityAdress { get; set; }
        public string StreetAdress { get; set; }

        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        public string WelcomeTittle { get; set; }
        public string WelcomeDesc1 { get; set; }
        public string WelcomeDesc2 { get;set; }
        public string? WelcomeImage { get; set; }

        [NotMapped]
        public IFormFile? welcomeImageFormFile { get; set; }

        public string WelcomeButtonContent { get; set; }

        public string WelcomeButtonLink { get; set; }

        public string NoticeVideoLink { get; set; }
        public string NoticeText { get; set; }

        public string MailFooter { get; set; }

        public string SiteFooter { get; set; }

        public string? IconAdress { get; set; }
        [NotMapped]
        public IFormFile? AdressIconFormFile { get; set; }
        public string? IconPhone { get; set; }
        [NotMapped]
        public IFormFile? PhoneIconFormFile { get; set; }
        public string? IconMail { get; set; }
        [NotMapped]
        public IFormFile? MailIconFormFile { get; set; }
    }
}
