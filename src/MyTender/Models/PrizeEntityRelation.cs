using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTender.Models
{
    public class PrizeEntityRelation
    {
        public int Id { get; set; }

        public int PrizeId { get; set; }

        public string EntityId { get; set; }
        public string EntityType { get; set; }
    }
}
