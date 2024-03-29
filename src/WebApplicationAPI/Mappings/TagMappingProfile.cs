using AutoMapper;
using WebApplicationAPI.Dtos;
using WebApplicationAPI.Models;

namespace WebApplicationAPI.Mappings
{
    public class TagMappingProfile : Profile
    {
        public TagMappingProfile()
        {
            CreateMap<Tag, TagDto>()
                .ConstructUsing(src => new TagDto(src.Name, src.Count, src.Percentage));
        }
    }
}
