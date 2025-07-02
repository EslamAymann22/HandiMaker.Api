namespace HandiMaker.Data.Entities.ProductClasses
{
    public class ProductPicture : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string PictureUrl { get; set; }

    }
}
