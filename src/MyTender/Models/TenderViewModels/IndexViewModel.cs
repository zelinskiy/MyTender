using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTender.Models.TenderViewModels
{
    public class IndexViewModel
    {
        public List<Tender> Tenders { get; set; }
        public ApplicationUser Me { get; set; }
    }
}
