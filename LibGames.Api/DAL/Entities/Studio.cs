
namespace LibGames.Api.DAL.Entities;

public class Studio
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Game> Games { get; set; }
}
