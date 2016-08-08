using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTender.Models
{
    public class Prize
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        //Plz access these from the other side
        public virtual TenderResponce TenderResponce { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Tender Tender { get; set; }
    }
}
