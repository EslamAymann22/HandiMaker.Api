using HandiMaker.Data.Entities.UserClassese;

namespace HandiMaker.Data.Entities.PostClasses
{
    public class Post : BaseEntity
    {
        public string? Content;
        public List<PostPicture> postPictures;
        public DateTime CreatedAt { get; set; }
        public AppUser? PostOwner { get; set; } ///////ConfigDone
        public string? PostOwnerId { get; set; }///////ConfigDone
        public List<AppUser?> ReactedUsers { get; set; } = new List<AppUser?>();///////ConfigDone

        public List<Comment> Comments { get; set; }///////ConfigDone
    }
}
