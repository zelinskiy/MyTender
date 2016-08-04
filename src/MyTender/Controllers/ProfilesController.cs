using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyTender.Data;
using MyTender.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MyTender.Controllers
{
    [Authorize]
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
            ViewData["Title"] = "All Profiles";
            var model = _context.Users
                .Include(u => u.Tenders)
                .Include(u => u.TenderResponces)
                .ToList();
            return View(model);
        }

        public IActionResult Profile(string id)
        {
            ViewData["Title"] = "Profile";
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
                return View("Index", new List<ApplicationUser>() { model });
            }            
        }

        [Authorize(Roles ="admin")]
        public async Task<IActionResult> ToggleAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (!(user.UserName == "zelinskiy1917@gmail.com"))
            {
                await _userManager.AddToRoleAsync(user, "admin");
            }            
            return RedirectToAction("Profile", new { id = id });
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddMoney(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.Money += 1000;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Profile", new { id = id });
        }
    }
}