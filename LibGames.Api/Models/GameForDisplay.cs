namespace LibGames.Api.Models;

public class GameForDisplay
{
    public string SearchQuery { get; set; }
    public List<GameDto> Games { get; set; }
}
