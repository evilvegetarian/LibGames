using LibGames.Api.DAL.Data;
using LibGames.Api.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace LibGames.Api.DAL;

public interface IGenreRepository
{
    Task<Genre> CreateGenreAsync(Genre genre, CancellationToken ct);
    Task DeleteGenryAsync(Genre genre, CancellationToken ct);
    Task<Genre?> GetGenreAsync(int id, CancellationToken ct);
    Task<List<Genre>> GetGenresAsync(CancellationToken ct);
    Task<Genre> UpdateGenreAsync(Genre genre, CancellationToken ct);
    Task<ICollection<Genre>> GetGenresAsyncForGame(List<int> idGenres, CancellationToken ct);
}

public class GenreRepository : IGenreRepository
{
    private readonly GameDbContext dbContext;

    public GenreRepository(GameDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Genre>> GetGenresAsync(CancellationToken ct)
    {
        return await dbContext.Genres.AsNoTracking().ToListAsync(ct);
    }

    public async Task<Genre?> GetGenreAsync(int id, CancellationToken ct)
    {
        return await dbContext.Genres
                              .Include(x => x.Games)
                              .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<ICollection<Genre>> GetGenresAsyncForGame(List<int> idGenres, CancellationToken ct)
    {
        return await dbContext.Genres
                              .Where(g => idGenres.Contains(g.Id))
                              .ToListAsync(ct);
    }

    public async Task<Genre> CreateGenreAsync(Genre genre, CancellationToken ct)
    {
        var creategenre = await dbContext.Genres.AddAsync(genre, ct);
        await dbContext.SaveChangesAsync(ct);
        return creategenre.Entity;
    }

    public async Task<Genre> UpdateGenreAsync(Genre genre, CancellationToken ct)
    {
        var updateGenre = dbContext.Genres.Update(genre);
        await dbContext.SaveChangesAsync(ct);
        return updateGenre.Entity;
    }

    public async Task DeleteGenryAsync(Genre genre, CancellationToken ct)
    {
        dbContext.Genres.Remove(genre);
        await dbContext.SaveChangesAsync(ct);
    }
}
