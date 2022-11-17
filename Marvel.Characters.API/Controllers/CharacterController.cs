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
    }
}
