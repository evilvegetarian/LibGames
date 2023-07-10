using AutoMapper;
using LibGames.Api.BL.Exc;
using LibGames.Api.BL.Services;
using LibGames.Api.DAL.Entities;
using LibGames.Api.Models;
using Microsoft.AspNetCore.Mvc;


namespace LibGames.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GenreController : ControllerBase
{
    private readonly ILogger<GenreController> logger;
    private readonly IGenreService genreService;
    private readonly IMapper mapper;

    public GenreController(ILogger<GenreController> logger, IGenreService genreService, IMapper mapper)
    {
        this.genreService = genreService;
        this.mapper = mapper;
        this.logger = logger;
    }

    // GET: api/<GenreController>
    [HttpGet]
    public async Task<ActionResult<List<GenreDto>>> Get(CancellationToken ct)
    {
        try
        {
            var genre = await genreService.GetGenresAsync(ct);
            var genreMap = mapper.Map<List<GenreDto>>(genre);
            return Ok(genreMap);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting genres");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // GET api/<GenreController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDto>> Get(int id, CancellationToken ct)
    {
        try
        {
            var genre = await genreService.GetGenreAsync(id, ct);

            var genreMap = mapper.Map<GenreDto>(genre);
            return Ok(genreMap);
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "Not Found exception occurred while getting genre with ID {ID}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting genre with ID {ID}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }

    }

    // POST api/<GenreController>
    [HttpPost]
    public async Task<ActionResult<GenreDto>> Post([FromBody] GenreCreatDto genreCreatDto, CancellationToken ct)
    {
        try
        {
            var genreMap = mapper.Map<Genre>(genreCreatDto);
            var genre = await genreService.CreateGenreAsync(genreMap, ct);
            return CreatedAtAction("Get", new { genre.Id }, genreCreatDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating genre {@Genre}", genreCreatDto);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // PUT api/<GenreController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] GenreCreatDto genreCreatDto, CancellationToken ct)
    {
        try
        {
            var genreMap = mapper.Map<Genre>(genreCreatDto);
            await genreService.UpdateGenryAsync(id, genreMap, ct);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "Not Found exception occurred while updating genre with ID {ID}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating genre with ID {ID}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }


    }

    // DELETE api/<GenreController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            await genreService.DeleteGenryAsync(id, ct);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "Not Found exception occurred while deleting genre with ID {ID}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting genre with ID {ID}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
