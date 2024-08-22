using _Models.RelationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MinLength(5, ErrorMessage = "Your Name Should Be Longer Than 4 Characters")]
        [Remote("CheckName","Home",AdditionalFields = "Bio" , ErrorMessage ="Invalid Name")]
        public string Fullname { get; set; }

        public DateOnly BirthDate { get; set; }

        public string? ProfilePicUrl { get; set; }
        public double? ProfilePicMargin { get; set; }

        public string? BackgroundPicUrl { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Your Bio Should Be Longer Than 10 Characters")]
        public string Bio {  get; set; }


        public ICollection<User_Friend> Users { get; set; }
        public ICollection<User_Friend> Friends { get; set; }
        public ICollection<User_VoteUps> Voters {  get; set; }
        public ICollection<User_Posts> Saver { get; set; }
    }
}
