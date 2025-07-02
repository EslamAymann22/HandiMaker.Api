namespace HandiMaker.Data.Entities.UserClassese
{
    public class UserFollow
    {
        public string FollowerId { get; set; }
        public AppUser Follower { get; set; }

        public string FollowingId { get; set; }
        public AppUser Following { get; set; }

        public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
    }
}
