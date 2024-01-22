using Commerce.Data;
using Commerce.Models;
using Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var cartProducts =await context.Cart.Where(c => c.UserId == user.Id)
                .Select(p=>new {p.ProductId,p.UserId}).Distinct().ToListAsync();

            var products = new List<ProductViewModel>();

            double totalPrice = 0;

            foreach (var item in cartProducts)
            {
                var product = await context.Product.FindAsync(item.ProductId);

                var prodCount = context.Cart.Where(c => c.ProductId == product.Id).ToList().Count;

                var productvm = new ProductViewModel
                {
                    Name = product.Name,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                    Id = product.Id,
                    ProductCount = prodCount,
                    ProductImage = product.ProductImage,
                    TotalPrice=(product.Price*prodCount)
                };

                products.Add(productvm);
            }
            foreach (var item in products) totalPrice += item.TotalPrice;

            var vm = new CartViewModel
            {
                Products = products,
                TotalProducts = products.Count,
                TotalPrice = totalPrice,
            };

            return View(vm);
        }

        public async Task<IActionResult> RemoveAll()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var cartProducts = await context.Cart.Where(c => c.UserId == user.Id).ToListAsync();

            foreach (var item in cartProducts)
            {
                context.Cart.Remove(item);
                context.SaveChanges();
            }

            return RedirectToAction("Index","Products");
        }


            public async Task <IActionResult> AddToCart(int productId)
        {
            
            var product = await context.Product.FindAsync(productId);

            if (product is null)
                return NotFound();

            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (user is null)
                return NotFound();

            var itemCount = context.Cart.Where(c => c.ProductId == productId).ToList().Count;

            var cart = new Cart { ProductId = productId, UserId = user.Id};

            await context.Cart.AddAsync(cart);
            context.SaveChanges();

            return RedirectToAction(nameof(Index),"Cart");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || context.Cart == null)
            {
                return NotFound();
            }

            var cartItem = await context.Cart
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            context.Cart.Remove(cartItem);
            context.SaveChanges();

            return RedirectToAction(nameof(Index), "Cart");
        }
    }
}
