using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Models.RelationModels
{
    [PrimaryKey(nameof(UserId), nameof(postId))]
    public class User_VoteUps
    {
        public string UserId { get; set; }
        public ApplicationUser user { get; set; }

        public int postId { get; set; }
        public Post Post { get; set; }
    }
}
