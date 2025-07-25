namespace HandiMaker.Data.Entities.ChatClasses
{
    public class MessageDocs : BaseEntity
    {
        public string? DocumentUrl { get; set; }
        public Message Message { get; set; }
        public int MessageId { get; set; }

    }
}
