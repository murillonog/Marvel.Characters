using AutoMapper;
using Marvel.Api;
using Marvel.Api.Filters;
using Marvel.Api.Results;
using Marvel.Characters.Application.Dtos;
using Marvel.Characters.Application.Exceptions;
using Marvel.Characters.Application.Filters;
using Marvel.Characters.Application.Interfaces;
using Marvel.Characters.Domain.Entities;
using Marvel.Characters.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Marvel.Characters.Application.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly string publicKey = "78d65d9d20c5eda1cbf25aa249e3efc1";
        private readonly string privateKey = "1b3e05739f5487c29050c90c8aa0740f6532cddd";
        private readonly int limit = 100;
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CharacterService> _logger;

        public CharacterService(ICharacterRepository characterRepository, IMapper mapper)
        {
            _characterRepository = characterRepository;
            _mapper = mapper;
        }

        public async Task FavoriteCharacter(CharacterDto characterDto)
        {
            try
            {
                characterDto.Favorite = true;
                var entity = _mapper.Map<Character>(characterDto);
                await _characterRepository.Update(entity);

            }
            catch (Exception exception)
            {
                var errMsg = new ErrorMessage("01.01", "Unexpected error to favorite character.");
                _logger.LogError(exception, HanddleError<object>.Handle(characterDto, exception, errMsg));
                throw new ServiceException(errMsg.Code, errMsg.Message, exception);
            }
        }

        public async Task<CharacterDto?> GetCharacterDetails(int id)
        {
            try
            {
                var result = await _characterRepository.GetById(id);
                return _mapper.Map<CharacterDto>(result);
            }
            catch (Exception exception)
            {
                var errMsg = new ErrorMessage("01.02", "Unexpected error to get character details.");
                _logger.LogError(exception, HanddleError<object>.Handle(id, exception, errMsg));
                throw new ServiceException(errMsg.Code, errMsg.Message, exception);
            }
        }

        public async Task<CharacterResultDto> GetCharacters(CharacterFilter filters)
        {
            try
            {
                var list = await _characterRepository.Get(filters.GetClauseWhere(), filters.Page.Value, filters.Size.Value);
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
            catch (Exception exception)
            {
                var errMsg = new ErrorMessage("01.03", "Unexpected error to get list characters.");
                _logger.LogError(exception, HanddleError<object>.Handle(filters, exception, errMsg));
                throw new ServiceException(errMsg.Code, errMsg.Message, exception);
            }
        }

        public async Task<int> QuantityFavorite()
        {
            try
            {
                return await _characterRepository.GetTotalFavorites();
            }
            catch (Exception exception)
            {
                var errMsg = new ErrorMessage("01.04", "Unexpected error to get total characters favorited.");
                _logger.LogError(exception, HanddleError<object>.Handle(null, exception, errMsg));
                throw new ServiceException(errMsg.Code, errMsg.Message, exception);
            }
        }

        public async Task UnfavoriteCharacter(CharacterDto characterDto)
        {
            try
            {
                characterDto.Favorite = false;
                var entity = _mapper.Map<Character>(characterDto);
                await _characterRepository.Update(entity);
            }
            catch (Exception exception)
            {
                var errMsg = new ErrorMessage("01.05", "Unexpected error to unfavorite character.");
                _logger.LogError(exception, HanddleError<object>.Handle(characterDto, exception, errMsg));
                throw new ServiceException(errMsg.Code, errMsg.Message, exception);
            }
        }

        public async Task<string> SyncDataBase()
        {
            try
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
                    return "Database successfully synced!";
                }
                return "The database was previously synchronized!";
            }
            catch (Exception exception)
            {
                var errMsg = new ErrorMessage("01.06", "Unexpected error to sync database.");
                _logger.LogError(exception, HanddleError<object>.Handle(null, exception, errMsg));
                throw new ServiceException(errMsg.Code, errMsg.Message, exception);
            }
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
