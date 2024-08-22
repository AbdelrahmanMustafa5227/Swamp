using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using _Models.RelationModels;

namespace _Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [MinLength(10 , ErrorMessage = "Post is too Short")]
        public string Body { get; set; }

        public string userId { get; set; }

        public DateTime PostDate { get; set; }

        public int Loves { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(userId))]
        public ApplicationUser Poster { get; set; }

        public ICollection<User_VoteUps> postsVotedUp { get; set; }
        public ICollection<User_Posts> postsSaved { get; set; }

    }
}
