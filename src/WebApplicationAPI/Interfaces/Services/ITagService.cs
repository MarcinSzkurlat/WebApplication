using WebApplicationAPI.Dtos;

namespace WebApplicationAPI.Interfaces.Services
{
    public interface ITagService
    {
        public Task<IEnumerable<TagDto>> GetTagsAsync();
        public Task DeleteTagsAsync();
    }
}
