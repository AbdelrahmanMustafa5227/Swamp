using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Models.ViewModels
{
    public class ChatVM
    {
        public ApplicationUser Sender { get; set; }
        public ApplicationUser Reciever { get; set; }
    }
}
