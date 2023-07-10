using AutoMapper;
using LibGames.Api.BL.Exc;
using LibGames.Api.BL.ResourceParameters;
using LibGames.Api.BL.Services;
using LibGames.Api.DAL.Entities;
using LibGames.Api.Models;
using Microsoft.AspNetCore.Mvc;


namespace LibGames.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GameController : ControllerBase
{
    private readonly IGameService gameService;
    private readonly ILogger<GameController> logger;
    private readonly IMapper mapper;

    public GameController(IGameService gameService, ILogger<GameController> logger, IMapper mapper)
    {
        this.gameService = gameService;
        this.logger = logger;
        this.mapper = mapper;
    }

    // GET: api/<GameController>
    [HttpGet]
    public async Task<ActionResult<GameForDisplay>> Get(int pageNumber, int pageSize, string? searchQuery, CancellationToken ct)
    {
        try
        {
            var parameters = new GameResourceParameters { PageNumber = pageNumber, PageSize = pageSize, seachQuery = searchQuery };
            var games = await gameService.GetGamesAsync(parameters, ct);
            var gameMap = mapper.Map<List<GameDto>>(games);
            return Ok(new GameForDisplay { Games = gameMap, SearchQuery = searchQuery });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Not Found exception occurred while getting games.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // GET api/<GameController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GameDto>> Get(int id, CancellationToken ct)
    {
        try
        {
            var game = await gameService.GetGameAsync(id, ct);
            var gameMap = mapper.Map<GameDto>(game);
            return Ok(gameMap);
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "Not Found exception occurred while getting game with ID {ID}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting game with ID {ID}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // POST api/<GameController>
    [HttpPost]
    public async Task<ActionResult<GameDto>> Post([FromBody] GameCreateDto gameCreateDto, CancellationToken ct)
    {
        try
        {
            var game = mapper.Map<Game>(gameCreateDto);
            var gameCreate = await gameService.CreateGameAsync(game, gameCreateDto.IdGenre, gameCreateDto.IdStudios, ct);
            var gameMap = mapper.Map<GameDto>(gameCreate);

            return CreatedAtAction("Get", new { id = gameMap.Id }, gameCreateDto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while create game");
            return StatusCode(500, "An error occurred while processing your request.");

        }
    }

    // PUT api/<GameController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<GameDto>> Put(int id, [FromBody] GameCreateDto gameCreateDto, CancellationToken ct)
    {
        try
        {
            var game = mapper.Map<Game>(gameCreateDto);
            game.Id = id;
            await gameService.UpdateGameAsync(id, gameCreateDto.IdGenre, gameCreateDto.IdStudios, game, ct);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "Not Found exception occurred while updating game with ID {ID}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating game with ID {ID}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // DELETE api/<GameController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            await gameService.DeleteGameAsync(id, ct);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "Not Found exception occurred while deleting game with ID {ID}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting game with ID {ID}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
