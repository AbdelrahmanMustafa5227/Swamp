using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Models.ViewModels
{
    public class PostVM
    {
        public Post post { get; set; }
        public List<Comment> comments { get; set; }
    }
}
