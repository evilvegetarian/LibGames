using LibGames.Api.BL.Exc;
using LibGames.Api.DAL;
using LibGames.Api.DAL.Entities;


namespace LibGames.Api.BL.Services;

public interface IStudioService
{
    Task<Studio> CreateStudioAsync(Studio studio, CancellationToken ct);
    Task DeleteStudioAsync(int id, CancellationToken ct);
    Task<List<Studio>> GetStudiosAsync(CancellationToken ct);
    Task<Studio?> GetStudioAsync(int id, CancellationToken ct);
    Task<Studio> UpdateStudioAsync(int id, Studio studio, CancellationToken ct);
}

public class StudioService : IStudioService
{
    private readonly IStudioRepository studioRepository;
    private readonly ILogger<StudioService> logger;

    public StudioService(IStudioRepository studioRepository, ILogger<StudioService> logger)
    {
        this.studioRepository = studioRepository;
        this.logger = logger;
    }

    public async Task<List<Studio>> GetStudiosAsync(CancellationToken ct)
    {
        logger.LogInformation("Getting all studios");
        return await studioRepository.GetStudiosAsync(ct);
    }

    public async Task<Studio?> GetStudioAsync(int id, CancellationToken ct)
    {
        logger.LogInformation("Getting studio with ID {ID}", id);
        var studio = await studioRepository.GetStudioAsync(id, ct)
              ?? throw new NotFoundException("Studio", id);
        return studio;
    }

    public async Task<Studio> CreateStudioAsync(Studio studio, CancellationToken ct)
    {
        logger.LogInformation("Creating studio {@Studio}", studio);
        return await studioRepository.CreateStudioAsync(studio, ct);
    }

    public async Task<Studio> UpdateStudioAsync(int id, Studio studio, CancellationToken ct)
    {
        logger.LogInformation("Updating studio with ID {ID}", id);
        var existingStudio = await studioRepository.GetStudioAsync(id, ct)
            ?? throw new NotFoundException("Studio", id);

        existingStudio.Games = studio.Games;
        existingStudio.Name = string.IsNullOrWhiteSpace(studio.Name) ? existingStudio.Name : studio.Name;
        existingStudio.Description = string.IsNullOrWhiteSpace(studio.Description) ? existingStudio.Description : studio.Description;

        return await studioRepository.UpdateStudioAsync(existingStudio, ct);
    }

    public async Task DeleteStudioAsync(int id, CancellationToken ct)
    {
        logger.LogInformation("Deleting studio with ID {ID}", id);
        var studio = await studioRepository.GetStudioAsync(id, ct)
                    ?? throw new NotFoundException("Studio", id);

        await studioRepository.DeleteStudioAsync(studio, ct);
    }
}
