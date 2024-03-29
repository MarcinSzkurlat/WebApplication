using WebApplicationAPI.Models;

namespace WebApplicationAPI.Interfaces.Repositories
{
    public interface ITagRepository
    {
        public Task SaveChangesAsync();
        public Task<IEnumerable<Tag>> GetTagsAsync(int pageNumber, int pageSize, SortBy sortBy, SortDirection sortDirection);
        public Task<int> GetTagsCountAsync();
        public Task DeleteTagsAsync();
        public Task AddTagsAsync(List<Tag> tags);
    }
}
