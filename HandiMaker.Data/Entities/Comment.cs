using HandiMaker.Data.Entities.PostClasses;

namespace HandiMaker.Data.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public AppUser? CommentOwner { get; set; }///////ConfigDone
        public string? CommentOwnerId { get; set; }///////ConfigDone

        public Post? Post { get; set; }///////ConfigDone
        public int? PostId { get; set; }///////ConfigDone


        public int? ParentId { get; set; }
        public Comment? Parent { get; set; }

        public List<Comment>? Children { get; set; } = new();

    }
}
