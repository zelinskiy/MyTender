using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyTender.Models.ManageViewModels
{
    public class ChangeAvatarViewModel
    {
        [DataType(DataType.ImageUrl)]
        public string AvatarUrl { get; set; }
    }
}
