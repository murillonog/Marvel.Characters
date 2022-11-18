using Marvel.Characters.Application.Dtos;
using Marvel.Characters.Application.Filters;

namespace Marvel.Characters.Application.Interfaces
{
    public interface ICharacterService
    {
        Task<string> SyncDataBase();
        Task<CharacterResultDto> GetCharacters(CharacterFilter filter);
        Task<CharacterDto?> GetCharacterDetails(int id);
        Task FavoriteCharacter(CharacterDto characterDto);
        Task UnfavoriteCharacter(CharacterDto characterDto);
        Task<int> QuantityFavorite();
    }
}
