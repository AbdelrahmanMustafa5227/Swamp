using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Models.ViewModels
{
    public class ProfileVM
    {
        public ApplicationUser user { get; set; }
        public Post? newpost { get; set; }
        public IEnumerable<Post> postsHistory { get; set; }
    }
}
