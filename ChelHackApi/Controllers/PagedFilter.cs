namespace ChelHackApi.Controllers
{
    public class PagedFilter
    {

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string TextFilter { get; set; }
        public string SortField { get; set; } = "Price";
        public string SortOrder { get; set; } = "asc";
    }
}