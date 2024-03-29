using AutoMapper;
using Serilog;
using WebApplicationAPI.Dtos;
using WebApplicationAPI.Interfaces.Repositories;
using WebApplicationAPI.Interfaces.Services;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagService(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedItems<IEnumerable<TagDto>>> GetTagsAsync(int pageNumber, int pageSize, SortBy sortBy, SortDirection sortDirection)
        {
            if (pageNumber == 0) throw new BadHttpRequestException("Page number must be more than 0");

            var tags = await _tagRepository.GetTagsAsync(pageNumber, pageSize, sortBy, sortDirection);
            var tagsDto = _mapper.Map<IEnumerable<TagDto>>(tags);

            var totalTagsCount = await _tagRepository.GetTagsCountAsync();

            return new PaginatedItems<IEnumerable<TagDto>>(
                tagsDto,
                pageNumber,
                (int)Math.Ceiling(totalTagsCount / (double)pageSize));
        }

        public async Task DeleteTagsAsync()
        {
            await _tagRepository.DeleteTagsAsync();

            Log.Information("Tags deleted from database");
        }
    }
}
