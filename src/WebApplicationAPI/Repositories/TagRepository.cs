using System.Linq.Expressions;
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

        public async Task<IEnumerable<Tag>> GetTagsAsync(int pageNumber, int pageSize, SortBy sortBy, SortDirection sortDirection)
        {
            List<Tag> tags;

            Expression<Func<Tag, object>> selectedColumn = sortBy == SortBy.Percentage ? x => x.Percentage : x => x.Name;

            if (sortDirection == SortDirection.ASC)
            {
                tags = await _dbContext.Tags
                    .OrderBy(selectedColumn)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                tags = await _dbContext.Tags
                    .OrderByDescending(selectedColumn)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToListAsync();
            }

            return tags;
        }

        public async Task<int> GetTagsCountAsync()
        {
            return await _dbContext.Tags.CountAsync();
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
