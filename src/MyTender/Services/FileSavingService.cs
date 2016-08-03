using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyTender.Services
{
    public class FileSavingService
    {
        private IHostingEnvironment hostingEnvironment;
        private Random random;

        public FileSavingService(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
            random = new Random();
        }

        public async Task<string> SaveFileAsync(IFormFile file)
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


            string uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");

            using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine("uploads", filename);
        }

    }
}
