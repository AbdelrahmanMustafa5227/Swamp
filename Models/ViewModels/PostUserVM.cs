using _Models.RelationModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Models.ViewModels
{
    public class PostUserVM
    {
        public ApplicationUser user { get; set; }
        public PostVM postvm {  get; set; }
        public bool IsLiked { get; set; }
        public bool IsSaved { get; set; }
    }
}
