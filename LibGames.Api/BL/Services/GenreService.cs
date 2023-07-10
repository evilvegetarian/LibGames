using LibGames.Api.BL.Exc;
using LibGames.Api.DAL;
using LibGames.Api.DAL.Entities;


namespace LibGames.Api.BL.Services;

public interface IGenreService
{
    Task<Genre> CreateGenreAsync(Genre genre, CancellationToken ct);
    Task DeleteGenryAsync(int id, CancellationToken ct);
    Task<List<Genre>> GetGenresAsync(CancellationToken ct);
    Task<Genre?> GetGenreAsync(int id, CancellationToken ct);
    Task<Genre> UpdateGenryAsync(int id, Genre genre, CancellationToken ct);
}

public class GenreService : IGenreService
{
    private readonly IGenreRepository genreRepository;
    private readonly ILogger<GenreService> logger;

    public GenreService(IGenreRepository genreRepository, ILogger<GenreService> logger)
    {
        this.genreRepository = genreRepository;
        this.logger = logger;
    }

    public async Task<List<Genre>> GetGenresAsync(CancellationToken ct)
    {
        logger.LogInformation("Getting all genres");
        return await genreRepository.GetGenresAsync(ct);

    }

    public async Task<Genre?> GetGenreAsync(int id, CancellationToken ct)
    {
        logger.LogInformation("Getting genre with ID {ID}", id);
        var genre = await genreRepository.GetGenreAsync(id, ct)
            ?? throw new NotFoundException("Genre", id);
        return genre;
    }

    public async Task<Genre> CreateGenreAsync(Genre genre, CancellationToken ct)
    {
        logger.LogInformation("Creating genre {@Genre}", genre);
        var createdGenre = await genreRepository.CreateGenreAsync(genre, ct);
        return createdGenre;
    }

    public async Task<Genre> UpdateGenryAsync(int id, Genre genre, CancellationToken ct)
    {
        logger.LogInformation("Updating genre with ID {ID}", id);
        var existingGenre = await genreRepository.GetGenreAsync(id, ct)
            ?? throw new NotFoundException("Genre", id);
        existingGenre.Games = genre.Games;
        existingGenre.Name = string.IsNullOrWhiteSpace(genre.Name) ? existingGenre.Name : genre.Name;

        var updatedGenre = await genreRepository.UpdateGenreAsync(existingGenre, ct);
        return updatedGenre;
    }

    public async Task DeleteGenryAsync(int id, CancellationToken ct)
    {
        logger.LogInformation("Deleting genre with ID {ID}", id);
        var genre = await genreRepository.GetGenreAsync(id, ct)
                    ?? throw new NotFoundException("Genre", id);

        await genreRepository.DeleteGenryAsync(genre, ct);
    }

}
