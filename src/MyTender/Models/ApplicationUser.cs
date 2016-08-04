using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MyTender.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, ISearchable
    {
        public virtual List<Tender> Tenders { get; set; }
        public virtual List<TenderResponce> TenderResponces { get; set; }
        public int Money { get; set; }

        public string AvatarUrl { get; set; }

        public List<string> GetSearchableFields()
        {
            return new List<string>()
            {
                UserName,
                Email
            };
        }
        

    }
}
