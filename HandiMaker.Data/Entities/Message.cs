namespace HandiMaker.Data.Entities
{
    public class Message : BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string? Content { get; set; } = "";
        public string? FileUrl { get; set; }
        public bool HasFile { get; set; } = false;
        public DateTime CreateAt { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public bool IsSeen { get; set; } = false;
    }
}
