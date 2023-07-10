namespace LibGames.Api.Models;

public class StudioDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<GameForGenrStudDto> Games { get; set; }
}
