namespace API.Helpers
{
    public class PaginationHeader
    {
        public PaginationHeader(int currentPage, int itemsperPage, int totalItems, int totalPages)
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsperPage;
            TotalPages = totalPages;
            TotalItems = totalItems;
        }

        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}