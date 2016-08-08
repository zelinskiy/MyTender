using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyTender.Models.TenderViewModels
{
    public class AssignPrizeViewModel
    {
        public List<Prize> PrizesList { get; set; }

        [Required]
        public int PrizeId { get; set; }

        [Required]
        public int EntityId { get; set; }
    }
}
