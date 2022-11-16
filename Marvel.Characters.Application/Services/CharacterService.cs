using Marvel.Api;
using Marvel.Api.Filters;
using Marvel.Api.Results;
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

        public CharacterService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public async Task SyncDataBase()
        {

            var list = await _characterRepository.GetAll();

            if (!list.Any())
            {
                int count = 1;
                var options = new CharacterRequestFilter();
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

        private async Task SeedData(CharacterResult response)
        {
            foreach (var item in response.Data.Results)
            {
                try
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
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
    }
}
