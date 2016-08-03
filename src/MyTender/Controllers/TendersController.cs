using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyTender.Data;
using MyTender.Models;
using Microsoft.AspNetCore.Authorization;
using MyTender.Models.TenderViewModels;

namespace MyTender.Controllers
{
    [Authorize]
    public class TendersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TendersController(ApplicationDbContext context)
        {
            _context = context;
        }

        private ApplicationUser Me
        {
            get
            {
                return _context.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            }
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel();
            model.Tenders = await _context.Tenders
                .Include(t => t.Author)
                .Include(t => t.Responces)
                .ToListAsync();
            model.Me = Me;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tender model)
        {
            if(model.Price > Me.Money)
            {
                ModelState.AddModelError("", $"Price is {model.Price}, but you have only {Me.Money}");
            }            
            if (ModelState.IsValid)
            {
                model.Author = Me;
                model.IsActive = true;
                Me.Money -= model.Price;
                _context.Users.Update(Me);
                _context.Tenders.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }            
        }


        [HttpGet]
        public async Task<IActionResult> Close(int id)
        {
            var responce = await _context.TenderResponces
                .Include(t=>t.Author)
                .Include(t => t.Tender)
                .SingleOrDefaultAsync(t => t.Id == id);
            if(responce == null)
            {
                return NotFound();
            }
            responce.Author.Money += responce.Tender.Price;
            _context.Update(responce.Author);
            responce.Tender.IsActive = false;
            _context.Update(responce.Tender);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Tender(int id)
        {
            var tender = await _context.Tenders
                .Include(t=>t.Author)
                .SingleOrDefaultAsync(t => t.Id == id);

            var responces = await _context.TenderResponces
                .Include(r => r.Author)
                .Where(r => r.Tender.Id == tender.Id)
                .ToListAsync();

            var model = new TenderViewModel()
            {
                Tender = tender,
                Responces = responces,
                Me = this.Me
            };
            return View(model);
        }
        

        [HttpPost]
        public async Task<IActionResult> AddResponce(int id,TenderResponce model)
        {
            if (ModelState.IsValid)
            {                
                var tender = await _context.Tenders
                    .SingleOrDefaultAsync(t => t.Id == id);
                model.Id = 0;
                model.Tender = tender;
                model.Author = Me;                
                _context.TenderResponces.Add(model);
                await _context.SaveChangesAsync();                
                return RedirectToAction("Tender", new { id = id });
            }
            else
            {
                return RedirectToAction("Tender", new { id = id });
            }
            
        }


    }
}
