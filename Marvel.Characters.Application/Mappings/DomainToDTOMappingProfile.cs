using AutoMapper;
using Marvel.Characters.Application.Dtos;
using Marvel.Characters.Domain.Entities;

namespace Marvel.Characters.Application.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Character, CharacterDto>().ReverseMap();
        }
    }
}
