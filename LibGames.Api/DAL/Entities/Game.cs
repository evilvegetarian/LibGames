
namespace LibGames.Api.DAL.Entities;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime YearOfRelease { get; set; }


    public ICollection<Studio> Studios { get; set; }
    public ICollection<Genre> Genres { get; set; }

}
