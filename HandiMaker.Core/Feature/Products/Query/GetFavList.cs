using HandiMaker.Core.ResponseBase.Paginations;
using MediatR;

namespace HandiMaker.Core.Feature.Products.Query
{
    class GetFavListModel : PaginationParams, IRequest<PaginatedResponse<GetProductsDto>>
    {
        public string? AuthorizeEmail { get; set; }
    }

    //class GetFavListHandler : IRequestHandler<GetFavListModel, PaginatedResponse<GetProductsDto>>
    //{
    //    public Task<PaginatedResponse<GetProductsDto>> Handle(GetFavListModel request, CancellationToken cancellationToken)
    //    {

    //    }
    //}
}
