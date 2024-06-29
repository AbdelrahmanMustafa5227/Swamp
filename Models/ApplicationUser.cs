using _Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Fullname { get; set; }

        public DateOnly BirthDate { get; set; }

        public string? ProfilePicUrl { get; set; }

        public string Bio {  get; set; }

        public ICollection<User_Friend> Users { get; set; }
        public ICollection<User_Friend> Friends { get; set; }


    }
}
