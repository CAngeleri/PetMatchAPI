#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MomosMatch.Models
{

public class UserLogin {

        [Display(Name = "Email")]
        public string LoginEmail { get; set; }

        [Display(Name = "Password")]
         public string LoginPassword { get; set; }
}

}