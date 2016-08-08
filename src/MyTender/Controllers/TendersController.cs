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
using Microsoft.AspNetCore.Identity;

namespace MyTender.Controllers
{
    [Authorize]
    public class TendersController : Controller
    {
        private readonly ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;

        public TendersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Tender model)
        {
            if (model.Price > Me.Money)
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
            responce.IsWinner = true;
            _context.Update(responce.Author);
            responce.Tender.IsActive = false;
            _context.Update(responce.Tender);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AssignPrizeTender(int id)
        {
            ViewData["ActionName"] = "AssignPrizeTender";
            var model = new AssignPrizeViewModel();
            var tender = _context.Tenders
                .Single(t => t.Id == id);

            var availablePrizesIds = _context.PrizeEntityRelations
                .Where(pr => pr.EntityType == "ApplicationUSer"
                    && pr.EntityId == Me.Id)
                .Select(pr => pr.PrizeId);

            var prizes = _context.Prizes
                .Where(p => availablePrizesIds.Contains(p.Id))
                .ToList();

            model.PrizesList = prizes;
            model.EntityId = id;

            return View("AssignPrize", model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignPrizeTender(int id, AssignPrizeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tender = _context.Tenders
                    .Single(t => t.Id == id);

                var pr = _context.PrizeEntityRelations
                    .First(x => x.EntityType == "ApplicationUser"
                        && x.EntityId == Me.Id
                        && x.PrizeId == model.PrizeId);

                _context.PrizeEntityRelations.Remove(pr);
                _context.PrizeEntityRelations.Add(new PrizeEntityRelation()
                {
                    PrizeId = model.PrizeId,
                    EntityType = "Tender",
                    EntityId = tender.Id.ToString()
                });

                _context.SaveChanges();

                return RedirectToAction("Tender", new { id = model.EntityId });
            }
            else
            {
                return View("AsignPrize", model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> AssignPrizeResponce(int id)
        {
            ViewData["ActionName"] = "AssignPrizeResponce";
            var model = new AssignPrizeViewModel();
            var responce = _context.TenderResponces
                .Include(r => r.Tender)
                .Single(r => r.Id == id);

            var availablePrizesIds = _context.PrizeEntityRelations
                .Where(pr => pr.EntityType == "Tender"
                    && pr.EntityId == responce.Tender.Id.ToString())
                .Select(pr => pr.PrizeId);

            var prizes = _context.Prizes
                .Where(p => availablePrizesIds.Contains(p.Id))
                .ToList();

            model.PrizesList = prizes;
            model.EntityId = id;

            return View("AssignPrize", model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignPrizeResponce(int id, AssignPrizeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var responce = _context.TenderResponces
                    .Include(r => r.Tender)
                    .Include(r=>r.Author)
                    .Single(r => r.Id == model.EntityId);

                var pr = _context.PrizeEntityRelations
                    .First(x => x.EntityType == "Tender"
                        && x.EntityId == responce.Tender.Id.ToString()
                        && x.PrizeId == model.PrizeId);

                _context.PrizeEntityRelations.Remove(pr);
                _context.PrizeEntityRelations.Add(new PrizeEntityRelation()
                {
                    PrizeId = model.PrizeId,
                    EntityType = "TenderResponce",
                    EntityId = responce.Id.ToString()
                });
                _context.PrizeEntityRelations.Add(new PrizeEntityRelation()
                {
                    PrizeId = model.PrizeId,
                    EntityType = "ApplicationUser",
                    EntityId = responce.Author.Id.ToString()
                });

                _context.SaveChanges();

                return RedirectToAction("Tender", new { id = responce.Tender.Id });
            }
            else
            {
                return View("AsignPrize", model);
            }
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

            var prizesIds = _context.PrizeEntityRelations
                .Where(pr => pr.EntityType == "Tender"
                    && pr.EntityId == tender.Id.ToString())
                .Select(pr => pr.PrizeId);

            var prizes = _context.Prizes
                .Where(p => prizesIds.Contains(p.Id))
                .ToList();

            foreach (var r in responces)
            {
                r.likes = _context.Likes
                    .Count(l => l.TenderResponceId == r.Id);
                r.likedByMe = _context.Likes
                    .Count(l => l.TenderResponceId == r.Id 
                        && l.ApplicationUserId == Me.Id) != 0;

                var rPrizesIds = _context.PrizeEntityRelations
                    .Where(pr => pr.EntityType == "TenderResponce"
                        && pr.EntityId == r.Id.ToString())
                    .Select(pr => pr.PrizeId);

                r.prizes = _context.Prizes
                    .Where(p => rPrizesIds.Contains(p.Id))
                    .ToList();
            }

            var model = new TenderViewModel()
            {
                Tender = tender,
                Responces = responces,
                Me = this.Me,
                Prizes = prizes
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


        [HttpGet]
        public async Task<IActionResult> ToggleLike(int id)
        {
            var user = Me;
            var responce = await _context.TenderResponces
                .Include(t=>t.Author)
                .Include(t => t.Tender)
                .Include(t=>t.Tender.Responces)
                .SingleOrDefaultAsync(r => r.Id == id);


            var responces = responce.Tender.Responces;
            var likesInTender = _context.Likes
                .Count(l => l.ApplicationUserId == user.Id
                    && responces.Exists(r => r.Id == l.TenderResponceId));

            var like = await _context.Likes
                .SingleOrDefaultAsync(l => l.TenderResponceId == id 
                    && l.ApplicationUserId == user.Id);
            if (like == null)
            {
                if (likesInTender < 3)
                {
                    _context.Likes.Add(new Like()
                    {
                        ApplicationUserId = user.Id,
                        TenderResponceId = id
                    });
                }               
            }
            else
            {
                _context.Likes.Remove(like);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Tender", new { id = responce.Tender.Id });
        }


        [HttpGet]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> DeleteTender(int id)
        {
            var tender = await _context.Tenders
                .Include(t=>t.Responces)
                .SingleOrDefaultAsync(t => t.Id == id);
            var responces = tender.Responces;
            var likes = _context.Likes
                .Where(l => responces.Exists(r => r.Id == l.TenderResponceId));

            _context.Likes.RemoveRange(likes);
            _context.TenderResponces.RemoveRange(responces);
            _context.Tenders.Remove(tender);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteTenderResponce(int id)
        {
            var responce = await _context.TenderResponces
                .Include(tr=>tr.Tender)
                .SingleOrDefaultAsync(tr => tr.Id == id);
            var likes = _context.Likes
                .Where(l => l.TenderResponceId == responce.Id);
            int tenderId = responce.Tender.Id;

            _context.Likes.RemoveRange(likes);
            _context.TenderResponces.Remove(responce);
            await _context.SaveChangesAsync();

            return RedirectToAction("Tender", new { id = tenderId });
        }
                

    }
}
