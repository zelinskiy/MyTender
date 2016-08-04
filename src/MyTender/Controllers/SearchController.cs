using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyTender.Data;
using Microsoft.AspNetCore.Identity;
using MyTender.Models;
using Microsoft.EntityFrameworkCore;
using MyTender.Models.SearchViewModels;

namespace MyTender.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {

        ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index(IndexViewModel model)
        {
            if (string.IsNullOrEmpty(model.Pattern))            
                return View(model);
            
            if (model == null) model = new IndexViewModel();
            var all = new List<ISearchable>();

            if (model.IsTender) all.AddRange(_context.Tenders
                .Where(t => t.IsActive == true || model.IsActive == false)
                .ToList());

            if (model.IsTenderResponce) all.AddRange(_context.TenderResponces
                 .Include(tr => tr.Tender)
                 .Where(t => t.Tender.IsActive == true || model.IsActive == false)
                 .ToList());

            if (model.IsUser) all.AddRange(_context.Users
                 .ToList());            

            model.Result = all.Where(o => o.GetSearchableFields().Exists(f => f.Contains(model.Pattern))).ToList();

            return View(model);
        }

    }
}