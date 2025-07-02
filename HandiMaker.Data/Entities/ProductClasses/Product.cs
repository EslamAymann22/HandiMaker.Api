using HandiMaker.Data.Entities.UserClassese;

namespace HandiMaker.Data.Entities.ProductClasses
{
    public class Product : BaseEntity
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public List<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
        public List<ProductPicture> ProductPictures { get; set; } = new List<ProductPicture>();
        public string Size { get; set; }
        public DateOnly DeliveryAt { get; set; }

        public AppUser? Owner { get; set; }///////ConfigDone
        public string? OwnerId { get; set; }///////ConfigDone

        public List<AppUser> FavAt { get; set; } = new List<AppUser>();///////ConfigDone



    }
}
