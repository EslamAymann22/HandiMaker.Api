using HandiMaker.Data.Entities.UserClassese;

namespace HandiMaker.Data.Entities
{
    public class Notification : BaseEntity
    {
        public string Content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime NoteAt { get; set; }

        public string? UserId { get; set; }///////ConfigDone
        public AppUser User { get; set; } ///////ConfigDone
    }
}
