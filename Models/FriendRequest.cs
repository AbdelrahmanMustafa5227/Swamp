using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Models
{
    public class FriendRequest
    {
        public int Id { get; set; }

        
        public string FromId { get; set; }
        [ForeignKey("FromId")]
        public ApplicationUser FromUser { get; set; }

        
        public string ToId { get; set; }
        [ForeignKey("ToId")]
        public ApplicationUser ToUser { get; set; }

        public string Message { get; set; }

        public DateTime SendDate { get; set; }
    }
}
