namespace HandiMaker.Core.ResponseBase.Paginations
{
    public class PaginatedResponse<T>
    {
        public PaginatedResponse(List<T> data)
        {
            PaginatedData = data;
        }
        public List<T> PaginatedData { get; set; }

        internal PaginatedResponse(bool Success, List<T> data = default, List<string> messages = null, int count = 0, int page = 1, int pageSize = 10)
        {
            PaginatedData = data;
            CurrentPage = page;
            IsSuccess = Success;
            PageSize = pageSize;
            TotalPages = (count + pageSize - 1) / pageSize;
            TotalCount = count;
        }

        public static PaginatedResponse<T> Create(List<T> data, int count, int page, int pageSize)
        {
            return new(true, data, null, count, page, pageSize);
        }

        public static PaginatedResponse<T> Fail(List<string> messages)
        {
            return new PaginatedResponse<T>(false, default, messages, 0, 0, 0);
        }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public object Meta { get; set; }

        public int PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;

        public List<string> Messages { get; set; } = new();

        public bool IsSuccess { get; set; }
    }

}
