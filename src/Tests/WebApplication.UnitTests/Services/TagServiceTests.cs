using AutoMapper;
using Microsoft.AspNetCore.Http;
using WebApplicationAPI.Dtos;
using WebApplicationAPI.Interfaces.Repositories;
using WebApplicationAPI.Models;
using WebApplicationAPI.Services;

namespace WebApplication.UnitTests.Services
{
    public class TagServiceTests
    {
        private Mock<IMapper> _mockMapper;
        private Mock<ITagRepository> _mockTagRepository;
        private readonly SortBy SortBy = SortBy.Percentage;
        private readonly SortDirection SortDirection = SortDirection.DESC;
        private const int PageSize = 10;

        [SetUp]
        public void Setup()
        {
            _mockMapper = new Mock<IMapper>();
            _mockTagRepository = new Mock<ITagRepository>();
        }

        [Test]
        public async Task GetTagsAsync_ReturnsPaginatedItemsOfTagDto_AllParametersPassed()
        {
            const int pageNumber = 1;
            const int totalPages = 1;

            var expectedTags = new List<Tag>()
            {
                new Tag() { Id = 1, Name = "Tag1", Count = 10, Percentage = 50 },
                new Tag() { Id = 2, Name = "Tag2", Count = 10, Percentage = 50 }
            };

            var expectedTagDtos = new List<TagDto>()
            {
                new TagDto("Tag1", 10, 50),
                new TagDto("Tag2", 10, 50)

            };

            var expectedResult = new PaginatedItems<IEnumerable<TagDto>>(expectedTagDtos, pageNumber, totalPages);

            _mockTagRepository.Setup(x => x.GetTagsAsync(pageNumber, PageSize, SortBy, SortDirection))
                .ReturnsAsync(expectedTags);
            _mockTagRepository.Setup(x => x.GetTagsCountAsync()).ReturnsAsync(expectedTags.Count);
            _mockMapper.Setup(x => x.Map<IEnumerable<TagDto>>(expectedTags)).Returns(expectedTagDtos);

            var tagService = new TagService(_mockTagRepository.Object, _mockMapper.Object);


            var result = await tagService.GetTagsAsync(pageNumber, PageSize, SortBy, SortDirection);


            Assert.NotNull(result.Items);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public async Task GetTagsAsync_ThrowBadHttpRequestException_PageNumberIsZero()
        {
            const int pageNumber = 0;

            var tagService = new TagService(_mockTagRepository.Object, _mockMapper.Object);


            Assert.That(async () => await tagService.GetTagsAsync(pageNumber, PageSize, SortBy, SortDirection), Throws.TypeOf<BadHttpRequestException>());
        }

        [Test]
        public async Task DeleteTagsAsync_DeletesTagsFromRepository_WhenTrigger()
        {
            _mockTagRepository.Setup(x => x.DeleteTagsAsync()).Returns(Task.CompletedTask);

            var tagService = new TagService(_mockTagRepository.Object, _mockMapper.Object);


            await tagService.DeleteTagsAsync();


            _mockTagRepository.Verify(x => x.DeleteTagsAsync(), Times.Once);
        }
    }
}
