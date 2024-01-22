using Commerce.Models;

namespace Commerce.ViewModels
{
    public class CartViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public int TotalProducts { get; set; }
        public double TotalPrice { get; set; }
    }
}
