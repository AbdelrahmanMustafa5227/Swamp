using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ApplicationUser : IdentityUser
    {
 
        public DateOnly BirthDate { get; set; }

        public string ProfilePicUrl { get; set; }


    }
}
