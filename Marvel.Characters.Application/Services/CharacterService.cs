using AutoMapper;
using Marvel.Api;
using Marvel.Api.Results;
using Marvel.Characters.Application.Dtos;
using Marvel.Characters.Application.Filters;
using Marvel.Characters.Application.Interfaces;
using Marvel.Characters.Domain.Entities;
using Marvel.Characters.Domain.Interfaces;

namespace Marvel.Characters.Application.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly string publicKey = "78d65d9d20c5eda1cbf25aa249e3efc1";
        private readonly string privateKey = "1b3e05739f5487c29050c90c8aa0740f6532cddd";
        private readonly int limit = 100;
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;

        public CharacterService(ICharacterRepository characterRepository, IMapper mapper)
        {
            _characterRepository = characterRepository;
            _mapper = mapper;
        }

        public async Task FavoriteCharacter(int id)
        {
            await _characterRepository.FavoriteCharacter(id);
        }

        public async Task<CharacterDto?> GetCharacterDetails(int id)
        {
            var result = await _characterRepository.GetById(id);
            return _mapper.Map<CharacterDto>(result);
        }

        public async Task<CharacterResultDto> GetCharacters(CharacterRequestFilter filters)
        {
            string filterCmd = filters.GetClauseWhere();

            var list = await _characterRepository.Get(filterCmd, filters.Page.Value, filters.Size.Value);
            var total = await _characterRepository.GetTotalCharacters();

            return new CharacterResultDto
            {
                Page = filters.Page,
                Size = filters.Size,
                Total = total,
                Count = list.ToList().Count(),
                Results = _mapper.Map<List<Character>, List<CharacterDto>>(list.ToList())
            };
        }

        public async Task<int> QuantityFavorite()
        {
            return await _characterRepository.GetTotalFavorites();
        }

        public async Task SyncDataBase()
        {
            var list = await _characterRepository.GetAll();

            if (!list.Any())
            {
                int count = 1;
                var options = new Marvel.Api.Filters.CharacterRequestFilter();
                options.Limit = limit;
                var client = new MarvelRestClient(publicKey, privateKey);
                var response = client.FindCharacters(options);

                await SeedData(response);

                var total = Convert.ToInt32(response.Data.Total);
                int quantity = total / limit;
                while (count <= quantity)
                {
                    options.Offset = limit * count;
                    response = client.FindCharacters(options);
                    await SeedData(response);
                    count++;
                }

            }
        }

        public async Task UnfavoriteCharacter(int id)
        {
            await _characterRepository.UnfavoriteCharacter(id);
        }

        private async Task SeedData(CharacterResult response)
        {
            foreach (var item in response.Data.Results)
            {
                var character = new Character(item.Name, item.Description, item.Modified, item.ResourceURI);
                var characterId = await _characterRepository.Add(character);

                character.Thumbnail = new Thumbnail(item.Thumbnail.Path, item.Thumbnail.Extension, characterId);
                item.Urls.ForEach(s => character.Urls.Add(new Url(s.URL, s.Type, characterId)));
                item.Comics.Items.ForEach(s => character.Comics.Add(new Comic(s.Name, s.ResourceURI, characterId)));
                item.Stories.Items.ForEach(s => character.Stories.Add(new Story(s.Name, s.ResourceURI, s.Type, characterId)));
                item.Events.Items.ForEach(s => character.Events.Add(new Event(s.Name, s.ResourceURI, characterId)));
                item.Series.Items.ForEach(s => character.Series.Add(new Series(s.Name, s.ResourceURI, characterId)));
                await _characterRepository.Update(character);
            }
        }
    }
}
