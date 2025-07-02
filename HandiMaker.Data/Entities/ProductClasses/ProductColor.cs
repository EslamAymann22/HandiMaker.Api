namespace HandiMaker.Data.Entities.ProductClasses
{
    public class ProductColor : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Color { get; set; }
    }
}
