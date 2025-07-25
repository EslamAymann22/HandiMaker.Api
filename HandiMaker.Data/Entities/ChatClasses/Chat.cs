namespace HandiMaker.Data.Entities.ChatClasses
{
    public class Chat : BaseEntity
    {

        public AppUser FUser { get; set; }
        public string FUserId { get; set; }
        public AppUser SUser { get; set; }
        public string SUserId { get; set; }
        public int NumberOfBendingMessages { get; set; } = 0;
        public string LastMessage { get; set; } = string.Empty;
        public DateTime LastMessageDate { get; set; }
        public string LastUserSenderId { get; set; }
        public ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}
