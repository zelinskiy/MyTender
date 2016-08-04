using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTender.Models.SearchViewModels
{
    public class IndexViewModel
    {
        //TODO: Nested Class

        public IndexViewModel()
        {
            Pattern = "";
            IsActive = IsTender = IsTenderResponce = IsUser = true;
            Result = new List<ISearchable>();
        }

        public string Pattern { get; set; }
        public bool IsActive { get; set; }
        public bool IsTender { get; set; }
        public bool IsTenderResponce { get; set; }
        public bool IsUser { get; set; }

        public List<ISearchable> Result { get; set; }
    }
}
