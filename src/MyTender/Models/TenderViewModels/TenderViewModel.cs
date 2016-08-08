using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTender.Models.TenderViewModels
{
    public class TenderViewModel
    {
        public Tender Tender { get; set; }
        public List<TenderResponce> Responces { get; set; }
        public List<Prize> Prizes { get; set; }
        public ApplicationUser Me { get; set; }
    }
}
