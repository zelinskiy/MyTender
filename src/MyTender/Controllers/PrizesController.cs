using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyTender.Models;
using Microsoft.AspNetCore.Identity;
using MyTender.Data;
using Microsoft.AspNetCore.Http;
using MyTender.Services;
using Microsoft.EntityFrameworkCore;

namespace MyTender.Controllers
{
    [Authorize]
    public class PrizesController : Controller
    {

        private readonly ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;
        FileSavingService _fileSaver;

        public PrizesController(ApplicationDbContext context,
            FileSavingService fileSaver,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _fileSaver = fileSaver;

        }

        private ApplicationUser Me
        {
            get
            {
                return _context.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            }
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Prize model, IFormFile file)
        {
            int CreationPrice = 100;
            if (CreationPrice > Me.Money)
            {
                ModelState.AddModelError("", $"You need at least {CreationPrice} to create new prize, but you have only {Me.Money}");
            }
            else if(file == null)
            {
                ModelState.AddModelError("", "Picture can't be empty");
            }
            if (ModelState.IsValid)
            {
                Me.Money -= CreationPrice;
                model.IsCreatedByUser = true;
                model.PictureUrl = await _fileSaver.SaveFileAsync(file);                
                _context.Prizes.Add(model);                                    
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }            
        }

        [HttpGet]
        public IActionResult Buy()
        {
            ViewData["Title"] = "Buy Prizes";
            ViewData["Message"] = "";
            return View(_context.Prizes.ToList());
        }

        [HttpPost]
        public IActionResult Buy(int id)
        {
            ViewData["Title"] = "Buy Prizes";
            var user = Me;
            var prize = _context.Prizes.Single(p => p.Id == id);
            if (prize.Price > user.Money)
            {
                ViewData["Message"] = "You dont have enough money";
            }
            else
            {
                ViewData["Message"] = $"You have bought {prize.Name}!";
                _context.PrizeEntityRelations.Add(new PrizeEntityRelation()
                {
                    PrizeId = id,
                    EntityId = user.Id,
                    EntityType = "ApplicationUser"
                });
                user.Money -= prize.Price;
                _context.SaveChanges();
            }
            return View(_context.Prizes.ToList());
        }
        

        public IActionResult My()
        {
            ViewData["Title"] = "My Prizes";
            ViewData["Message"] = "";

            var prizesIds = _context.PrizeEntityRelations
                .Where(pr => pr.EntityType == "ApplicationUser"
                    && pr.EntityId == Me.Id)
                .Select(pr => pr.PrizeId);

            var prizes = _context.Prizes
                .Where(p => prizesIds.Contains(p.Id))
                .ToList();

            foreach(var p in prizes)
            {
                p.quantity = prizesIds.Count(pid=> pid == p.Id);
            }

            return View("Buy", prizes);
        }


    }
}