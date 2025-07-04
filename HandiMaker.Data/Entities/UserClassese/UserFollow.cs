namespace HandiMaker.Data.Entities.UserClassese
{
    public class UserFollow
    {
        public string FollowerId { get; set; }
        public AppUser Follower { get; set; }

        public string FollowedId { get; set; }
        public AppUser Followed { get; set; }

        public DateTime FollowedAt { get; set; } = DateTime.UtcNow;
    }
}
