using Marvel.Characters.Application.Dtos;
using Marvel.Characters.Application.Exceptions;
using Marvel.Characters.Application.Filters;
using Marvel.Characters.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Marvel.Characters.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharactersController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpPut("sync-database")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> Sync()
        {
            try
            {
                var result = await _characterService.SyncDataBase();
                return Ok(result);
            }
            catch (ServiceException exception)
            {
                return Problem(exception.Details, null, 500, exception.DisplayMessage, exception.Error);
            }
            
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CharacterResultDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> Get([FromQuery] CharacterFilter filter)
        {
            try
            {
                var list = await _characterService.GetCharacters(filter);
                return Ok(list);
            }
            catch (ServiceException exception)
            {
                return Problem(exception.Details, null, 500, exception.DisplayMessage, exception.Error);
            }            
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CharacterDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _characterService.GetCharacterDetails(id);

                if (result is null)
                    return NotFound();

                return Ok(result);
            }
            catch (ServiceException exception)
            {
                return Problem(exception.Details, null, 500, exception.DisplayMessage, exception.Error);
            }
            
        }

        [HttpPatch("{id}/favorite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> Favorite(int id)
        {
            try
            {
                var character = await _characterService.GetCharacterDetails(id);

                if (character is null)
                    return NotFound();

                var total = await _characterService.QuantityFavorite();

                if (total >= 5)
                    return BadRequest("Already 5 characters favorited");

                await _characterService.FavoriteCharacter(character);

                return Ok();
            }
            catch (ServiceException exception)
            {
                return Problem(exception.Details, null, 500, exception.DisplayMessage, exception.Error);
            }
            
        }

        [HttpPatch("{id}/unfavorite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> Unfavorite(int id)
        {
            try
            {
                var character = await _characterService.GetCharacterDetails(id);

                if (character is null)
                    return NotFound();

                await _characterService.UnfavoriteCharacter(character);

                return Ok();
            }
            catch (ServiceException exception)
            {
                return Problem(exception.Details, null, 500, exception.DisplayMessage, exception.Error);
            }
            
        }
    }
}
