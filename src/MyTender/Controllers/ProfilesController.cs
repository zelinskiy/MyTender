using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyTender.Data;
using MyTender.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MyTender.Controllers
{
    public class ProfilesController : Controller
    {
        ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;

        public ProfilesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var model = _context.Users
                .Include(u => u.Tenders)
                .Include(u => u.TenderResponces)
                .ToList();
            return View(model);
        }

        public IActionResult Profile(string id)
        {
            var model = _context.Users
                .Include(u => u.Tenders)
                .Include(u => u.TenderResponces)
                .SingleOrDefault(u=>u.Id == id);

            if(model == null)
            {
                return NotFound(id);
            }
            else
            {
                return View(model);
            }
            
        }
    }
}