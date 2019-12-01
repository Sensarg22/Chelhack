namespace Common
{
    public interface IPaged
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }
}