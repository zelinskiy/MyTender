using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyTender.Models
{
    public class Prize
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name can't be empty")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Name length must be [1,1000]")]
        public string Name { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Description can't be empty")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description length must be [10,1000]")]
        public string Description { get; set; }

        [Required]
        [Range(20, 1000000, ErrorMessage = "Price must be [20, 10**6]")]
        public int Price { get; set; }
                
        public string PictureUrl { get; set; }

        public bool IsCreatedByUser { get; set; }
               
        public int quantity;
    }
}
