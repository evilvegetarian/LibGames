using LibGames.Api.DAL.Data;
using LibGames.Api.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace LibGames.Api.DAL;

public interface IGameRepository
{
    Task<Game> CreateGameAsync(Game game, CancellationToken ct);
    Task DeleteGameAsync(Game game, CancellationToken ct);
    Task<Game?> GetGameAsync(int id, CancellationToken ct);
    IQueryable<Game> GetGamesAsQueryable(string? searchQuery);
    Task<Game> UpdateGameAsync(Game game, CancellationToken ct);
}

public class GameRepository : IGameRepository
{
    private readonly GameDbContext dbContext;

    public GameRepository(GameDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IQueryable<Game> GetGamesAsQueryable(string? searchQuery)
    {
        var games = dbContext.Games.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            games = games.Where(x => x.Name.Contains(searchQuery));
        }

        return games.OrderBy(x => x.Name).AsQueryable();
    }

    public async Task<Game?> GetGameAsync(int id, CancellationToken ct)
    {
        return await dbContext.Games.Include(gen => gen.Genres)
                                    .Include(stu => stu.Studios)
                                    .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Game> CreateGameAsync(Game game, CancellationToken ct)
    {
        var gameCreate = await dbContext.Games.AddAsync(game, ct);
        await dbContext.SaveChangesAsync(ct);
        return gameCreate.Entity;
    }

    public async Task<Game> UpdateGameAsync(Game game, CancellationToken ct)
    {
        var gameUpdate = dbContext.Update(game);
        await dbContext.SaveChangesAsync(ct);
        return gameUpdate.Entity;
    }

    public async Task DeleteGameAsync(Game game, CancellationToken ct)
    {
        dbContext.Remove(game);
        await dbContext.SaveChangesAsync(ct);
    }
}
