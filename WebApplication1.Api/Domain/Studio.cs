namespace WebApplication1.API.Domain;

public class Studio : BaseEntity
{
    public required string Country { get; set; }
    public required ICollection<Game>? Games { get; set; } = new List<Game>();

    public Studio() : base()
    {
    }

    public Studio(string name) : base(name)
    {
        Name = name;
    }

    // Construtor com parâmetros para uso geral
    public Studio(string name, string country, ICollection<Game> games) : base(name)
    {
        Name = name;
        Country = country;
        Games = games;
    }
}