using LibGames.Api.BL.Exc;
using LibGames.Api.BL.Pagination;
using LibGames.Api.BL.ResourceParameters;
using LibGames.Api.DAL;
using LibGames.Api.DAL.Entities;


namespace LibGames.Api.BL.Services;

public interface IGameService
{
    Task DeleteGameAsync(int id, CancellationToken ct);
    Task<Game?> GetGameAsync(int id, CancellationToken ct);
    Task<PagedList<Game>> GetGamesAsync(GameResourceParameters parameters, CancellationToken ct);
    Task<Game> UpdateGameAsync(int id, List<int> idGenre, List<int> idStudios, Game game, CancellationToken ct);
    Task<Game> CreateGameAsync(Game game, List<int> idGenre, List<int> idStudios, CancellationToken ct);
}

public class GameService : IGameService
{
    private readonly IGameRepository gameRepository;
    private readonly ILogger<GameService> logger;
    private readonly IGenreRepository genreRepository;
    private readonly IStudioRepository studioRepository;

    public GameService(IGameRepository gameRepository,
                       ILogger<GameService> logger,
                       IGenreRepository genreRepository,
                       IStudioRepository studioRepository)
    {
        this.gameRepository = gameRepository;
        this.logger = logger;
        this.genreRepository = genreRepository;
        this.studioRepository = studioRepository;
    }

    public async Task<PagedList<Game>> GetGamesAsync(GameResourceParameters parameters, CancellationToken ct)
    {
        logger.LogInformation("Getting games with parameters {@Parameters}", parameters);
        var query = gameRepository.GetGamesAsQueryable(parameters.seachQuery);
        var pagedList = await PagedList<Game>.CreateAsync(query, parameters.PageNumber, parameters.PageSize, ct);
        return pagedList;
    }

    public async Task<Game?> GetGameAsync(int id, CancellationToken ct)
    {
        logger.LogInformation("Getting game with ID {ID}", id);
        var game = await gameRepository.GetGameAsync(id, ct)
            ?? throw new NotFoundException("Game", id);

        return game;
    }

    public async Task<Game> UpdateGameAsync(int id, List<int> idGenre, List<int> idStudios, Game game, CancellationToken ct)
    {
        logger.LogInformation("Updating game with ID {ID}", id);
        var existingGame = await gameRepository.GetGameAsync(id, ct)
            ?? throw new NotFoundException("Game", id);

        existingGame.Description = string.IsNullOrWhiteSpace(game.Description) ? existingGame.Description : game.Description;
        existingGame.Name = string.IsNullOrWhiteSpace(game.Name) ? existingGame.Name : game.Name;

        existingGame.YearOfRelease = game.YearOfRelease;
        existingGame.Studios = await studioRepository.GetStudioAsyncForGame(idStudios, ct);
        existingGame.Genres = await genreRepository.GetGenresAsyncForGame(idGenre, ct);

        var updatedGame = await gameRepository.UpdateGameAsync(existingGame, ct);

        return updatedGame;
    }

    public async Task<Game> CreateGameAsync(Game game, List<int> idGenre, List<int> idStudios, CancellationToken ct)
    {
        logger.LogInformation("Creating game {@Game}", game);
        game.Studios = await studioRepository.GetStudioAsyncForGame(idStudios, ct);
        game.Genres = await genreRepository.GetGenresAsyncForGame(idGenre, ct);
        return await gameRepository.CreateGameAsync(game, ct);
    }

    public async Task DeleteGameAsync(int id, CancellationToken ct)
    {
        logger.LogInformation("Deleting game with ID {ID}", id);
        var game = await gameRepository.GetGameAsync(id, ct)
                   ?? throw new NotFoundException("Game", id);

        await gameRepository.DeleteGameAsync(game, ct);
    }
}
