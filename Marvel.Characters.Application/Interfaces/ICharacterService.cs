using Marvel.Characters.Application.Dtos;
using Marvel.Characters.Application.Filters;

namespace Marvel.Characters.Application.Interfaces
{
    public interface ICharacterService
    {
        Task SyncDataBase();
        Task<CharacterResultDto> GetCharacters(CharacterRequestFilter filter);
    }
}
