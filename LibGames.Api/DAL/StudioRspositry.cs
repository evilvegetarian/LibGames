using LibGames.Api.DAL.Data;
using LibGames.Api.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace LibGames.Api.DAL;

public interface IStudioRepository
{
    Task<Studio> CreateStudioAsync(Studio studio, CancellationToken ct);
    Task DeleteStudioAsync(Studio studio, CancellationToken ct);
    Task<Studio?> GetStudioAsync(int id, CancellationToken ct);
    Task<List<Studio>> GetStudiosAsync(CancellationToken ct);
    Task<Studio> UpdateStudioAsync(Studio studio, CancellationToken ct);
    Task<ICollection<Studio>> GetStudioAsyncForGame(List<int> idStudious, CancellationToken ct);
}

public class StudioRepository : IStudioRepository
{
    private readonly GameDbContext dbContext;

    public StudioRepository(GameDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Studio>> GetStudiosAsync(CancellationToken ct)
    {
        return await dbContext.Studios.AsNoTracking().ToListAsync(ct);
    }

    public async Task<Studio?> GetStudioAsync(int id, CancellationToken ct)
    {
        return await dbContext.Studios
                              .Include(x => x.Games)
                              .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<ICollection<Studio>> GetStudioAsyncForGame(List<int> idStudious, CancellationToken ct)
    {
        return await dbContext.Studios.Where(g => idStudious.Contains(g.Id))
                                      .ToListAsync(ct);


    }

    public async Task<Studio> CreateStudioAsync(Studio studio, CancellationToken ct)
    {
        var createStudio = await dbContext.Studios.AddAsync(studio, ct);
        await dbContext.SaveChangesAsync(ct);
        return createStudio.Entity;
    }

    public async Task<Studio> UpdateStudioAsync(Studio studio, CancellationToken ct)
    {
        var updateStudio = dbContext.Studios.Update(studio);
        await dbContext.SaveChangesAsync(ct);
        return updateStudio.Entity;
    }

    public async Task DeleteStudioAsync(Studio studio, CancellationToken ct)
    {
        dbContext.Studios.Remove(studio);
        await dbContext.SaveChangesAsync(ct);
    }
}
