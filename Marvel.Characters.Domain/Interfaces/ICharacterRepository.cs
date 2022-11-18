using Marvel.Characters.Domain.Entities;

namespace Marvel.Characters.Domain.Interfaces
{
    public interface ICharacterRepository
    {
        Task<IEnumerable<Character>> GetAll();
        Task<int> GetTotalCharacters();
        Task<int> GetTotalFavorites();
        Task<int> Add(Character character);
        Task<Character> Update(Character character);
        Task<IEnumerable<Character>> Get(string cmd, int page, int size);
        Task<Character?> GetById(int id);
    }
}
