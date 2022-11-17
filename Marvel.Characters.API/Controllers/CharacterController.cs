using Marvel.Characters.Application.Dtos;
using Marvel.Characters.Application.Filters;
using Marvel.Characters.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Marvel.Characters.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpPut("sync-database")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> Sync()
        {
            await _characterService.SyncDataBase();
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CharacterResultDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> Get([FromQuery] CharacterRequestFilter filter)
        {
            var list = await _characterService.GetCharacters(filter);
            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CharacterDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _characterService.GetCharacterDetails(id);
            
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPatch("{id}/favorite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> Favorite(int id)
        {
            var result = await _characterService.GetCharacterDetails(id);

            if (result is null)
                return NotFound();

            var total = await _characterService.QuantityFavorite();

            if (total >= 5)
                return BadRequest("Already 5 characters favorited");

            await _characterService.FavoriteCharacter(id);

            return Ok();
        }

        [HttpPatch("{id}/unfavorite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
        public async Task<IActionResult> Unfavorite(int id)
        {
            var result = await _characterService.GetCharacterDetails(id);

            if (result is null)
                return NotFound();

            

            await _characterService.UnfavoriteCharacter(id);

            return Ok();
        }
    }
}
