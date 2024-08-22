using _Models.RelationModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Models.ViewModels
{
    public class HomeVM
    {
        public ApplicationUser user { get; set; }
        public List<PostVM> posts {  get; set; }
        public IEnumerable<int> voteUps { get; set; }
        public IEnumerable<int> savedPosts { get; set; }
    }
}
