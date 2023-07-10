namespace LibGames.Api.Models;

public class GameForGenrStudDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateOnly YearOfRelease { get; set; }
}
