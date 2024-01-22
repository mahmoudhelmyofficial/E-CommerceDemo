using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Commerce.Data;
using Commerce.Models;
using Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Commerce.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products= await _context.Product.Include(p => p.Category).ToListAsync();

            var categories = await _context.Category.ToListAsync();

            var prodsVM = new ProductsListVM { Products = products, Categories = categories };

            return View(prodsVM);
        }

        public async Task<IActionResult> Search(string searchKey) 
        {
            var products = await _context.Product
                .Where(s=>s.Name.Contains(searchKey)).Include(p => p.Category).ToListAsync();

            var categories = await _context.Category.ToListAsync();

            var prodsVM = new ProductsListVM { Products = products, Categories = categories };

            return View("Index",prodsVM);
        }

        public async Task<IActionResult> Filter(string filterCat)
        {
            var products = await _context.Product.Include(p => p.Category)
                .Where(s => s.Category.Name == filterCat).ToListAsync();

            var categories = await _context.Category.ToListAsync();

            var prodsVM = new ProductsListVM { Products = products, Categories = categories };

            return View("Index", prodsVM);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Product model)
        {
            using var dataStream = new MemoryStream();
            var file = Request.Form.Files;
            var poster = file.FirstOrDefault();

            if (file.Any())
                await poster.CopyToAsync(dataStream);



            var product = new Product
            {
                Name = model.Name,
                ProductImage = dataStream.ToArray(),
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId
            };


            _context.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
    
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id,Product model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            
            using var dataStream = new MemoryStream();
            var file = Request.Form.Files;
            var poster = file.FirstOrDefault();

            if (file.Any())
                await poster.CopyToAsync(dataStream);

            model.ProductImage = dataStream.ToArray();

            _context.Update(model);
            await _context.SaveChangesAsync();
        
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
