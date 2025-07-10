using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public ChangeReactionHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
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
                Post.ReactedUsers.Add(AuthorizedUser);
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
