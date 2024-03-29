using AutoMapper;
using WebApplicationAPI.Dtos;
using WebApplicationAPI.Interfaces.Repositories;
using WebApplicationAPI.Interfaces.Services;

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

        public async Task<IEnumerable<TagDto>> GetTagsAsync()
        {
            var tags = await _tagRepository.GetTagsAsync();
            var tagsDto = _mapper.Map<IEnumerable<TagDto>>(tags);

            return tagsDto;
        }

        public async Task DeleteTagsAsync()
        {
            await _tagRepository.DeleteTagsAsync();
        }
    }
}
