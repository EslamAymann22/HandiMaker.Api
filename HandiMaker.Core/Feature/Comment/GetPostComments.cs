using HandiMaker.Core.ResponseBase.Paginations;
using MediatR;

namespace HandiMaker.Core.Feature.Comment
{
    public class GetCommentDto
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public string CommentOwnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetPostCommentsModel : IRequest<PaginatedResponse<GetPostCommentsModel>>
    {

        public int PostId { get; set; }

    }


    public class GetPostCommentsHandler
    {
    }


}
