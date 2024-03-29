using WebApplicationAPI.Models;

namespace WebApplicationAPI.Interfaces.Repositories
{
    public interface ITagRepository
    {
        public Task SaveChangesAsync();
        public Task<IEnumerable<Tag>> GetTagsAsync();
        public Task DeleteTagsAsync();
        public Task AddTagsAsync(List<Tag> tags);
    }
}
