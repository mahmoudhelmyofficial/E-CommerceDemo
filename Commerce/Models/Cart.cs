namespace Commerce.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
