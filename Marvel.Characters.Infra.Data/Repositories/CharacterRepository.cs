using Dapper;
using Marvel.Characters.Domain.Entities;
using Marvel.Characters.Domain.Interfaces;
using Marvel.Characters.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Marvel.Characters.Infra.Data.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        protected ApplicationDbContext _context;
        protected DbSet<Character> DbSet;

        public CharacterRepository(ApplicationDbContext db)
        {
            _context = db;
            DbSet = _context.Set<Character>();
        }

        public async Task<int> Add(Character character)
        {
            var result = await DbSet.AddAsync(character);
            await _context.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task FavoriteCharacter(int id)
        {
            var sql = $"UPDATE Character SET Favorite = 1 WHERE Id = {id}";

            await _context.Database.GetDbConnection().QueryAsync<Character>(sql);
        }

        public async Task<IEnumerable<Character>> Get(string cmd, int page, int size)
        {
            var sql = $"SELECT * FROM Character {cmd}";

            var list = await _context.Database.GetDbConnection().QueryAsync<Character>(sql);

            return list.Skip(page * size).Take(size).Select<dynamic, Character>(row => GetDetails(row));
        }

        private Character GetDetails(dynamic row)
        {
            return new Character
            {
                Id = row.Id,
                Name = row.Name,
                Description = row.Description,
                ResourceURI = row.ResourceURI,
                Favorite = row.Favorite,
                Modified = row.Modified,

                Urls = GetUrls(row.Id),
                Thumbnail = GetThumbnail(row.Id),
                Comics = GetComics(row.Id),
                Stories = GetStories(row.Id),
                Events = GetEvents(row.Id),
                Series = GetSeries(row.Id)
            };
        }        

        private List<Url> GetUrls(int id)
        {
            return _context.Set<Url>().Where(c => c.CharacterId == id).ToList();
        }

        private Thumbnail? GetThumbnail(int id)
        {
            return _context.Set<Thumbnail>().Where(c => c.CharacterId == id).FirstOrDefault();
        }

        private List<Comic> GetComics(int id)
        {
            return _context.Set<Comic>().Where(c=> c.CharacterId == id).ToList();
        }

        private List<Story> GetStories(int id)
        {
            return _context.Set<Story>().Where(c => c.CharacterId == id).ToList();
        }

        private List<Event> GetEvents(int id)
        {
            return _context.Set<Event>().Where(c => c.CharacterId == id).ToList();
        }

        private List<Series> GetSeries(int id)
        {
            return _context.Set<Series>().Where(c => c.CharacterId == id).ToList();
        }

        public async Task<IEnumerable<Character>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Character?> GetById(int id)
        {
            return await DbSet.Where(c => c.Id == id)
                .Select(c => new Character()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ResourceURI = c.ResourceURI,
                    Modified = c.Modified,
                    Favorite = c.Favorite,
                    Urls = c.Urls,
                    Thumbnail = c.Thumbnail,
                    Comics = c.Comics,
                    Stories = c.Stories,
                    Events = c.Events,
                    Series = c.Series
                }).FirstOrDefaultAsync();
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
            await _context.Database.GetDbConnection()
                .QueryAsync<Character>(sql);
        }

        public async Task<Character> Update(Character obj)
        {
            _context.Update(obj);
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return obj;
        }
    }
}
