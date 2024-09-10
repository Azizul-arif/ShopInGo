using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerce.Data;
using Microsoft.Extensions.Hosting;
using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Controllers
{
    public class ProductImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductImagesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: ProductImages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductImages.Include(p => p.Product).ThenInclude(p => p.Category); ;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImages = await _context.ProductImages
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productImages == null)
            {
                return NotFound();
            }

            return View(productImages);
        }

        // GET: ProductImages/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImagePath,ProductId")] ProductImages productImages, List<IFormFile> Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string rPath = "";
                    string wwwRootPath = "";
                    if (_environment != null)
                    {
                        wwwRootPath = _environment.WebRootPath;
                        rPath = Path.Combine(wwwRootPath, "Images");
                    }
                    else
                    {
                        wwwRootPath = Directory.GetCurrentDirectory();
                        rPath = Path.Combine(wwwRootPath, "wwwRoot", "Images");
                    }

                    // Check if the image list is null or empty
                    if (Image == null || !Image.Any())
                    {
                        ModelState.AddModelError("", "No image files were uploaded.");
                        ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", productImages.ProductId);
                        return View(productImages);
                    }

                    foreach (var image in Image)
                    {
                        string extension = Path.GetExtension(image.FileName).ToLower();
                        if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                        {
                            string fileName = Path.GetFileNameWithoutExtension(image.FileName) + "_" + productImages.ProductId + extension;
                            string path = Path.Combine(rPath, fileName);

                            // Ensure the directory exists
                            if (!Directory.Exists(rPath))
                            {
                                Directory.CreateDirectory(rPath);
                            }

                            // Save the file
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await image.CopyToAsync(fileStream);
                            }

                            // Add image path to the database
                            var productImage = new ProductImages
                            {
                                ImagePath = "/Images/" + fileName,
                                ProductId = productImages.ProductId
                            };

                            _context.Add(productImage);
                            await _context.SaveChangesAsync();
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError("", ex.InnerException.Message);
                    }
                    else
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                    return View(productImages);
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                ModelState.AddModelError("", message);
                ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", productImages.ProductId);
                return View(productImages);
            }
        }


        // GET: ProductImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productImages = await _context.ProductImages.FindAsync(id);
        if (productImages == null)
        {
            return NotFound();
        }
        ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", productImages.ProductId);
        return View(productImages);
    }

    // POST: ProductImages/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,ImagePath,ProductId")] ProductImages productImages)
    {
        if (id != productImages.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(productImages);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductImagesExists(productImages.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", productImages.ProductId);
        return View(productImages);
    }

    // GET: ProductImages/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productImages = await _context.ProductImages
            .Include(p => p.Product)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (productImages == null)
        {
            return NotFound();
        }

        return View(productImages);
    }

    // POST: ProductImages/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var productImages = await _context.ProductImages.FindAsync(id);
        if (productImages != null)
        {
            _context.ProductImages.Remove(productImages);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductImagesExists(int id)
    {
        return _context.ProductImages.Any(e => e.Id == id);
    }
}
}
