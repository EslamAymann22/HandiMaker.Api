namespace HandiMaker.Data.Entities.PostClasses
{
    public class PostPicture : BaseEntity
    {
        public string PicturUrl { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; } // Navigation property to the Post entity
    }
}
