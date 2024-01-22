using Commerce.Models;

namespace Commerce.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public byte[] ProductImage { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public int ProductCount { get; set; }
        public double TotalPrice { get; set; }
    }
}
