using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTender.Models
{
    public interface ISearchable
    {
        List<string> GetSearchableFields();
    }
}
