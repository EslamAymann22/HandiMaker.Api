using HandiMaker.Data.Entities.ChatClasses;

namespace HandiMaker.Data.Entities
{
    public class Message : BaseEntity
    {
        public string? Content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime SeenAt { get; set; } = DateTime.MinValue;
        public ICollection<MessageDocs>? MessageDocs { get; set; } = new List<MessageDocs>();
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public DateTime CreateAt { get; set; }


        public AppUser? Sender { get; set; }///////ConfigDone
        public string? SenderId { get; set; }///////ConfigDone

        public AppUser? Receiver { get; set; }///////ConfigDone
        public string? ReceiverId { get; set; }///////ConfigDone


    }
}
