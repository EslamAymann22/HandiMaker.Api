namespace HandiMaker.Data.Entities
{
    public class Connection : BaseEntity
    {

        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string ConnectionId { get; set; }

    }
}
