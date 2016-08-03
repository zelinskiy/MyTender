using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTender.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int TenderResponceId { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
