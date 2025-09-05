namespace HandiMaker.Data.Entities
{
    public class Chat : BaseEntity
    {

        public string FUserId { get; set; }

        public string SUserId { get; set; }
        public AppUser FUser { get; set; }
        public AppUser SUser { get; set; }
        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
        public int? LastMessageId { get; set; }
        public Message? LastMessage { get; set; }
        public int NumberOfMessagesUnseen { get; set; }



    }
}
