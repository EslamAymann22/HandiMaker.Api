using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Data.Entities;
using HandiMaker.Data.Enums;
using HandiMaker.Infrastructure.DbContextData;
using HandiMaker.Services.Services.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Comments.Command
{
    public class AddCommentModel : IRequest<BaseResponse<string>>
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public int? ParentCommentId { get; set; }
        public string? AuthorizeEmail { get; set; }
    }

    public class AddCommentHandler : BaseResponseHandler, IRequestHandler<AddCommentModel, BaseResponse<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly HandiMakerDbContext _handiMakerDb;
        private readonly INotificationServices _notificationServices;

        public AddCommentHandler(UserManager<AppUser> userManager
            , HandiMakerDbContext handiMakerDb
            , INotificationServices notificationServices)
        {
            this._userManager = userManager;
            this._handiMakerDb = handiMakerDb;
            this._notificationServices = notificationServices;
        }
        public async Task<BaseResponse<string>> Handle(AddCommentModel request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.AuthorizeEmail ?? "");
            if (user is null)
                return Failed<string>(HttpStatusCode.Unauthorized);
            var post = await _handiMakerDb.Posts.FirstOrDefaultAsync(P => P.Id == request.PostId);
            if (post is null)
                return Failed<string>(HttpStatusCode.NotFound, "this post is not found");
            var NewComment = new Comment
            {
                Content = request.Content,
                CommentOwnerId = user.Id,
                ParentId = request.ParentCommentId,
                CreatedAt = DateTime.UtcNow,
                PostId = request.PostId
            };
            var parent = await _handiMakerDb.Comments.FindAsync(request.ParentCommentId ?? 0);

            try
            {
                await _handiMakerDb.Comments.AddAsync(NewComment);
                if (parent is not null)
                {
                    parent.NumOfChildren++;
                    _handiMakerDb.Comments.Update(parent);
                }
                await _handiMakerDb.SaveChangesAsync(cancellationToken);

                if (post.PostOwnerId != user.Id)
                {
                    var NotifiContent = $"{user.FirstName + " " + user.LastName} add comment in your post \n {NewComment.Content}";
                    var RouteLink = $"/api/Post/GetPostById?postId={post.Id}";
                    await _notificationServices.SendNotificationAsync(user, NotifiContent, post.PostOwnerId, RouteLink, NotifiType.NewComment);

                }
            }
            catch (Exception ex)
            {
                return Failed<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
            return Success("Comment Added!");
        }
    }

}
