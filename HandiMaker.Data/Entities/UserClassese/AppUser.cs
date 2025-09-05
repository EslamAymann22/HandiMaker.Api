using HandiMaker.Data.Entities.PostClasses;
using HandiMaker.Data.Entities.ProductClasses;
using HandiMaker.Data.Entities.UserClassese;
using HandiMaker.Data.Enums;
using Microsoft.AspNetCore.Identity;

namespace HandiMaker.Data.Entities
{
    public class AppUser : IdentityUser
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? BOD { get; set; }
        public UserRole Role { get; set; }
        public string? Location { get; set; }
        public string? PictureUrl { get; set; }
        public UserGender? Gender { get; set; }

        public List<Post> CreatedPosts { get; set; } = new();///////ConfigDone
        public List<Post> ReactedPosts { get; set; } = new();///////ConfigDone

        public List<Comment>? CreatedComments { get; set; }///////ConfigDone

        public List<Notification>? Notifications { get; set; }///////ConfigDone

        public List<Product>? Products { get; set; }///////ConfigDone
        public List<Product>? FavProducts { get; set; }///////ConfigDone

        public List<UserFollow>? Followers { get; set; }
        public List<UserFollow>? Following { get; set; }

        //public ICollection<Chat>? UserChats { get; set; } = new HashSet<Chat>();
        public ICollection<Message>? Messages { get; set; } = new HashSet<Message>();

        public ICollection<Connection>? Connections { get; set; } = new HashSet<Connection>();

    }
}
