using HandiMaker.Core.ResponseBase.GeneralResponse;
using HandiMaker.Infrastructure.DbContextData;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HandiMaker.Core.Feature.Comments.Command
{
    public class DeleteCommentModel : IRequest<BaseResponse<string>>
    {
        public int commentId { get; set; }
        public string? AuthorizeEmail { get; set; }
    }

    public class DeleteCommentHandler : BaseResponseHandler, IRequestHandler<DeleteCommentModel, BaseResponse<string>>
    {
        private readonly HandiMakerDbContext _handiMakerDb;

        public DeleteCommentHandler(HandiMakerDbContext handiMakerDb)
        {
            this._handiMakerDb = handiMakerDb;
        }
        public async Task<BaseResponse<string>> Handle(DeleteCommentModel request, CancellationToken cancellationToken)
        {
            var user = await _handiMakerDb.Users.FirstOrDefaultAsync(U => U.Email == (request.AuthorizeEmail ?? ""));
            if (user is null)
                return Failed<string>(HttpStatusCode.Unauthorized);
            var comment = await _handiMakerDb.Comments.FirstOrDefaultAsync(C => C.Id == request.commentId);
            if (comment is null)
                return Failed<string>(HttpStatusCode.NotFound, "This comment is not founded");

            if (comment.CommentOwnerId != user.Id)
                return Failed<string>(HttpStatusCode.Forbidden, "You aren't comment owner");

            var parent = await _handiMakerDb.Comments.FindAsync(comment.ParentId);

            try
            {
                _handiMakerDb.Comments.Remove(comment);
                if (parent is not null)
                {
                    parent.NumOfChildren--;
                    _handiMakerDb.Comments.Update(parent);
                }
                await _handiMakerDb.SaveChangesAsync(cancellationToken);
                return Success("Comment deleted!");
            }
            catch (Exception ex)
            {
                return Failed<string>(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

    }
}
