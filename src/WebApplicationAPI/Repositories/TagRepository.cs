using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.Interfaces.Repositories;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly WebApplicationDbContext _dbContext;

        public TagRepository(WebApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            return await _dbContext.Tags
                .ToListAsync();
        }

        public async Task DeleteTagsAsync()
        {
            await _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Tags");
        }

        public async Task AddTagsAsync(List<Tag> tags)
        {
            await _dbContext.Tags.AddRangeAsync(tags);
        }
    }
}
