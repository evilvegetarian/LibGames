
namespace LibGames.Api.BL.ResourceParameters;

public class GameResourceParameters
{
    private const int MaxPageSize = 20;
    private int pageSize = 10;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => pageSize;
        set => pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string seachQuery { get; set; }
}
