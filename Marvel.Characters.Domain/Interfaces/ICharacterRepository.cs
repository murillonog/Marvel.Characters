using Marvel.Characters.Domain.Entities;

namespace Marvel.Characters.Domain.Interfaces
{
    public interface ICharacterRepository
    {
        Task<IEnumerable<Character>> GetAll();
        Task<int> Add(Character character);
        Task<Character> Update(Character character);
    }
}
