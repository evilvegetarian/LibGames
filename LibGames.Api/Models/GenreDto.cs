namespace LibGames.Api.Models;

public class GenreDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<GameForGenrStudDto> Games { get; set; }

}
