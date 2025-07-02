using HandiMaker.Data.Entities.UserClassese;

namespace HandiMaker.Data.Entities
{
    public class Message : BaseEntity
    {
        public string? Content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreateAt { get; set; }
        public string? DocumentUrl { get; set; }


        public AppUser? Sender { get; set; }///////ConfigDone
        public string? SenderId { get; set; }///////ConfigDone

        public AppUser? Receiver { get; set; }///////ConfigDone
        public string? ReceiverId { get; set; }///////ConfigDone


    }
}
