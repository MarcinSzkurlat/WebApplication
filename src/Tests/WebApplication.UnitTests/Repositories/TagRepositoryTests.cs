using Microsoft.EntityFrameworkCore;
using WebApplicationAPI;
using WebApplicationAPI.Models;
using WebApplicationAPI.Repositories;

namespace WebApplication.UnitTests.Repositories
{
    public class TagRepositoryTests
    {
        private DbContextOptions<WebApplicationDbContext> _options;
        private const int PageNumber = 1;
        private const int PageSize = 10;
        private readonly List<Tag> _tags = new()
        {
            new Tag(){Id = 1, Name = "Tag1", Count = 30, Percentage = 30},
            new Tag(){Id = 2, Name = "Tag2", Count = 20, Percentage = 20},
            new Tag(){Id = 3, Name = "Tag3", Count = 50, Percentage = 50}
        };
        
        [SetUp]
        public async Task Setup()
        {
            _options = new DbContextOptionsBuilder<WebApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var dbContext = new WebApplicationDbContext(_options))
            {
                await dbContext.Tags.AddRangeAsync(_tags);
                await dbContext.SaveChangesAsync();
            }
        }

        [TearDown]
        public async Task Teardown()
        {
            await using (var dbContext = new WebApplicationDbContext(_options))
            {
                dbContext.Tags.RemoveRange(_tags);
                await dbContext.SaveChangesAsync();
            }
        }

        [TestCase(SortBy.Name, SortDirection.ASC, 1,3)]
        [TestCase(SortBy.Name, SortDirection.DESC, 3,1)]
        [TestCase(SortBy.Percentage, SortDirection.ASC, 2,3)]
        [TestCase(SortBy.Percentage, SortDirection.DESC, 3,2)]
        public async Task GetTagsAsync_ReturnsSortedTags_AppropriateParametersPassed(SortBy sortBy, SortDirection sortDirection, int expectedFirst, int expectedLast)
        {
            await using (var dbContext = new WebApplicationDbContext(_options))
            {
                var tagRepository = new TagRepository(dbContext);
                var result = await tagRepository.GetTagsAsync(PageNumber, PageSize, sortBy, sortDirection);

                Assert.AreEqual(expectedFirst, result.First().Id);
                Assert.AreEqual(expectedLast, result.Last().Id);
            }
        }

        [Test]
        public async Task GetTagsCountAsync_ReturnsTagsCount_WhenTrigger()
        {
            await using (var dbContext = new WebApplicationDbContext(_options))
            {
                var tagRepository = new TagRepository(dbContext);
                var result = await tagRepository.GetTagsCountAsync();

                Assert.AreEqual(3, result);
            }
        }
    }
}
