using AutoMapper;
using LibGames.Api.BL.Exc;
using LibGames.Api.BL.Services;
using LibGames.Api.DAL.Entities;
using LibGames.Api.Models;
using Microsoft.AspNetCore.Mvc;


namespace LibGames.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudioController : ControllerBase
{
    private readonly ILogger<StudioController> logger;
    private readonly IStudioService studioService;
    private readonly IMapper mapper;

    public StudioController(ILogger<StudioController> logger, IStudioService studioService, IMapper mapper)
    {
        this.studioService = studioService;
        this.mapper = mapper;
        this.logger = logger;
    }

    // GET: api/<StudioController>
    [HttpGet]
    public async Task<ActionResult<List<StudioDto>>> Get(CancellationToken ct)
    {
        try
        {
            var studios = await studioService.GetStudiosAsync(ct);
            var studiosMap = mapper.Map<List<StudioDto>>(studios);
            return Ok(studiosMap);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting studios");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // GET api/<StudioController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<StudioDto>> Get(int id, CancellationToken ct)
    {
        try
        {
            var studio = await studioService.GetStudioAsync(id, ct);
            var studioMap = mapper.Map<StudioDto>(studio);
            return Ok(studioMap);
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "Not Found exception occurred while getting studio with ID {ID}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting studio with ID {ID}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }

    }

    // POST api/<StudioController>
    [HttpPost]
    public async Task<ActionResult<StudioDto>> Post([FromBody] StudioCreatDto studioCreateDto, CancellationToken ct)
    {
        try
        {
            var studioMap = mapper.Map<Studio>(studioCreateDto);
            var studio = await studioService.CreateStudioAsync(studioMap, ct);
            return CreatedAtAction("Get", new { studio.Id }, studioCreateDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating studio {@Studio}", studioCreateDto);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // PUT api/<StudioController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] StudioCreatDto studioCreateDto, CancellationToken ct)
    {
        try
        {
            var studioMap = mapper.Map<Studio>(studioCreateDto);
            await studioService.UpdateStudioAsync(id, studioMap, ct);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "Not Found exception occurred while updating studio with ID {ID}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating studio with ID {ID}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // DELETE api/<StudioController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            await studioService.DeleteStudioAsync(id, ct);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "Not Found exception occurred while deleting studio with ID {ID}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting studio with ID {ID}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
