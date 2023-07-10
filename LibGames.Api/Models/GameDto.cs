namespace LibGames.Api.Models;

public class GameDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateOnly YearOfRelease { get; set; }

    public ICollection<StudioGameDto> Studios { get; set; }
    public ICollection<GenreGameDto> Genres { get; set; }
}
