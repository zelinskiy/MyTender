using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyTender.Models
{
    public class TenderResponce:ISearchable
    {
        public int Id { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Text can't be empty")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Length must be [2,1000]")]
        public string Text { get; set; }
        public bool IsWinner { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public virtual Tender Tender { get; set; }
   

        public int likes;
        public bool likedByMe;
        
        public List<string> GetSearchableFields()
        {
            return new List<string>()
            {
                Text
            };
        }
    }
}
