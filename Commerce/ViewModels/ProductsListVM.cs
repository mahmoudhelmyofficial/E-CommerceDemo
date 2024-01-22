using Commerce.Models;

namespace Commerce.ViewModels
{
    public class ProductsListVM
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
