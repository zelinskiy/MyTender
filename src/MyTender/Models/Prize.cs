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
        public virtual List<Prize> PrerequiredPrizes { get; set; }

        public string RewardedEntityType { get; set; }
        public int RewardedEntityId { get; set; }
    }
}
