namespace LibGames.Api.Models;

public class GameCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateOnly YearOfRelease { get; set; }


    public List<int> IdGenre { get; set; }
    public List<int> IdStudios { get; set; }
}
