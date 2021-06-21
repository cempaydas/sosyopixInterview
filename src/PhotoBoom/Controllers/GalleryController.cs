using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoBoom.Contracts;
using PhotoBoom.Models;

namespace PhotoBoom.Controllers
{
    public class GalleryController : Controller
    {

        private readonly IGalleryRepository _repo;


       

        public GalleryController(IGalleryRepository repo)
        {
            _repo = repo;
      
        }

        // GET: Gallery
        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        // GET: Gallery/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            } 

            var gallery = await _repo.GetItemAsync(id);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }

        // GET: Gallery/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gallery/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Tags,ImageFile")] Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = Directory.GetCurrentDirectory()+"/wwwroot";
                string extension = Path.GetExtension(gallery.ImageFile.FileName);
                string fileName = DateTime.Now.ToString("yymmssfff") + Guid.NewGuid() + extension;
                gallery.ImagePath = fileName;
                string path = Path.Combine(wwwRootPath + "/gallery/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await gallery.ImageFile.CopyToAsync(fileStream);
                }

                await _repo.CreateAsync(gallery);
                return RedirectToAction(nameof(Index));
            }
            return View(gallery);
        }





        // POST: Gallery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
             if (id == null)
            {
                return BadRequest();
            }
            var gallery = await _repo.GetItemAsync(id);
            if (gallery == null)
            {
                return NotFound();
            }
            var imagePath = Path.Combine( Directory.GetCurrentDirectory(), "/wwwroot/gallery", gallery.ImagePath);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            await _repo.DeleteAsync(gallery);
            return RedirectToAction(nameof(Index));
        }


    }
}
