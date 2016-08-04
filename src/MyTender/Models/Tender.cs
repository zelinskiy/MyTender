using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyTender.Models
{
    public class Tender:ISearchable
    {
        public int Id { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage ="Price must be [0, 10**4]")]
        public int Price { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public bool IsActive { get; set; }

        [Required(AllowEmptyStrings =false,ErrorMessage ="Text can't be empty")]
        [StringLength(1000,MinimumLength = 5, ErrorMessage ="Length must be [5,1000]")]
        public string Text { get; set; }

        public virtual List<TenderResponce> Responces { get; set; }

        public List<string> GetSearchableFields()
        {
            return new List<string>()
            {
                Text
            };
        }
    }
}
