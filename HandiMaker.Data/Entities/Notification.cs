using HandiMaker.Data.Enums;

namespace HandiMaker.Data.Entities
{
    public class Notification : BaseEntity
    {
        public string Content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime NoteAt { get; set; }

        public string? UserId { get; set; }///////ConfigDone
        public AppUser User { get; set; } ///////ConfigDone

        public string? NotifiedUserId { get; set; }

        public NotifiType NotifiType { get; set; }

        public string? RouteLink { get; set; }


    }
}
