using Commerce.Models;

namespace Commerce.ViewModels
{
    public class CategoriesVM
    {
        public Category Category { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
