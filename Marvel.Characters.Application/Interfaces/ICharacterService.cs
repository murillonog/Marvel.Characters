using Marvel.Characters.Application.Dtos;
using Marvel.Characters.Application.Filters;

namespace Marvel.Characters.Application.Interfaces
{
    public interface ICharacterService
    {
        Task SyncDataBase();
        Task<CharacterResultDto> GetCharacters(CharacterRequestFilter filter);
        Task<CharacterDto?> GetCharacterDetails(int id);
        Task FavoriteCharacter(int id);
        Task UnfavoriteCharacter(int id);
        Task<int> QuantityFavorite();
    }
}
