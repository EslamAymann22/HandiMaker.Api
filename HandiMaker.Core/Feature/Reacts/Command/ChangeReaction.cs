using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Infrastructure.DbContextData;
using HandiMaker.Services.Services.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace HandiMaker.Core.Feature.Reacts.Command
{
    public class ChangeReactionModel : IRequest<BaseResponse<string>>
    {
        public string? AuthorizeEmail { get; set; }
        public bool? AddReaction { get; set; } = true;
        public int PostId { get; set; }
    }

    public class ChangeReactionHandler : BaseResponseHandler, IRequestHandler<ChangeReactionModel, BaseResponse<string>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly INotificationServices _notificationServices;
        private readonly IConfiguration _configuration;

        public ChangeReactionHandler(HandiMakerDbContext handiMakerDb
            , INotificationServices notificationServices
            , IConfiguration configuration)
        {
            this._handiMakerDb = handiMakerDb;
            this._notificationServices = notificationServices;
            this._configuration = configuration;
        }

        public async Task<BaseResponse<string>> Handle(ChangeReactionModel request, CancellationToken cancellationToken)
        {
            var Post = await _handiMakerDb.Posts.Include(P => P.ReactedUsers).FirstOrDefaultAsync(P => P.Id == request.PostId);

            if (Post is null)
                return Failed<string>(HttpStatusCode.NotFound, "Post not found");
            var AuthorizedUser = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Email == (request.AuthorizeEmail ?? ""));

            if (AuthorizedUser is null)
                return Failed<string>(HttpStatusCode.Unauthorized, "Unauthorized user");

            var HasReaction = Post.ReactedUsers.Any(U => U.Id == AuthorizedUser.Id);
            if (request.AddReaction == HasReaction)
                return Failed<string>(HttpStatusCode.BadRequest, "You already make that to this post");

            if (request.AddReaction.Value)
            {
                Post.ReactedUsers.Add(AuthorizedUser);

                if (Post.PostOwnerId != AuthorizedUser.Id)
                {

                    await _notificationServices.SendNotificationAsync(AuthorizedUser,
                        $"{AuthorizedUser.FirstName + " " + AuthorizedUser.LastName} and {(Post.ReactedUsers.Count > 1 ? (Post.ReactedUsers.Count - 1 + " other ") : "")} React in your post \n {Post.Content ?? ""}",
                    Post.PostOwnerId, $"{_configuration["BaseUrl"]}/api/Post/GetPostById?postId={Post.Id}");
                }

            }
            else
                Post.ReactedUsers.Remove(AuthorizedUser);
            try
            {
                await _handiMakerDb.SaveChangesAsync(cancellationToken);
                return Success("Reaction added successfully");
            }
            catch (Exception ex)
            {
                return Failed<string>(HttpStatusCode.InternalServerError, $"An error occurred while adding reaction: {ex.Message}");
            }

        }
    }


}
