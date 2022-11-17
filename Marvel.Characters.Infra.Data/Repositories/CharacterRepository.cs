using Dapper;
using Marvel.Characters.Domain.Entities;
using Marvel.Characters.Domain.Interfaces;
using Marvel.Characters.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Marvel.Characters.Infra.Data.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        protected ApplicationDbContext Db;
        protected DbSet<Character> DbSet;

        public CharacterRepository(ApplicationDbContext db)
        {
            Db = db;
            DbSet = Db.Set<Character>();
        }

        public async Task<int> Add(Character character)
        {
            var result = await DbSet.AddAsync(character);
            await Db.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task FavoriteCharacter(int id)
        {
            var sql = $"UPDATE Character SET Favorite = 1 WHERE Id = {id}";
            await Db.Database.GetDbConnection()
                .QueryAsync<Character>(sql);
        }

        public async Task<IEnumerable<Character>> Get(string cmd, int page, int size)
        {
            var sql = $"SELECT * FROM Character {cmd}";

            var list = await Db.Database.GetDbConnection()
                .QueryAsync<Character>(sql);

            return list.Skip(page * size).Take(size);
        }

        public async Task<IEnumerable<Character>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Character?> GetById(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<int> GetTotalCharacters()
        {
            return await DbSet.CountAsync();
        }

        public async Task<int> GetTotalFavorites()
        {
            return await DbSet.CountAsync(f => f.Favorite);
        }

        public async Task UnfavoriteCharacter(int id)
        {
            var sql = $"UPDATE Character SET Favorite = 0 WHERE Id = {id}";
            await Db.Database.GetDbConnection()
                .QueryAsync<Character>(sql);
        }

        public async Task<Character> Update(Character obj)
        {
            Db.Update(obj);
            Db.Entry(obj).State = EntityState.Modified;
            await Db.SaveChangesAsync();
            return obj;
        }
    }
}
