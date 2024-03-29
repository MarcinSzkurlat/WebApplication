using WebApplicationAPI.Dtos;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Interfaces.Services
{
    public interface ITagService
    {
        public Task<PaginatedItems<IEnumerable<TagDto>>> GetTagsAsync(int pageNumber, int pageSize, SortBy sortBy, SortDirection sortDirection);
        public Task DeleteTagsAsync();
    }
}
