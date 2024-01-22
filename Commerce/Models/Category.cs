using System.Security.Permissions;

namespace Commerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Product { get; set; }
    }
}
