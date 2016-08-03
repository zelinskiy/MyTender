using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MyTender.Controllers
{
    public class FileController : Controller
    {
        private IHostingEnvironment hostingEnvironment;
        private Random random;

        public FileController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
            random = new Random();
        }


        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int len = 10;

            //Changing filename to random
            var filename = file.FileName;
            var extension = filename.Split('.').Last();
            filename = filename.Replace(extension, "");
            filename = new string(Enumerable.Repeat(chars, len)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            filename = filename + "." + extension;


            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");

            using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Json(filename);
        }
    }
}