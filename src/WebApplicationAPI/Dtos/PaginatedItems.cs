namespace WebApplicationAPI.Dtos
{
    public record PaginatedItems<T>(
        T Items,
        int PageNumber,
        int TotalPages);
}
