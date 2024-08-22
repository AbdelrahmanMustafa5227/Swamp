using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public string Body { get; set; }
        
        public int postId { get; set; }

        public DateTime date { get; set; }

        [ForeignKey("postId")]
        public Post post { get; set; }

        public string userId { get; set; }

        [ForeignKey("userId")]
        public ApplicationUser User { get; set; }
    }
}
