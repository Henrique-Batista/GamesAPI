using System.Diagnostics.CodeAnalysis;
using WebApplication1.API.Domain.Enums;
using Mapster;

namespace WebApplication1.API.Domain;

public class Game : BaseEntity
{
    public required DateTime ReleaseDate { get; set; }
    public required GameGender Gender { get; set; }
    public required Guid StudioId { get; set; }
    public Studio? Studio { get; set; }
    public required float Price { get; set; }

    public Game() : base()
    {
    }

    public Game(string name) : base(name)
    {
        Name = name;
    }
    
    // Construtor com parâmetros para uso geral
    public Game(string name, GameGender gender, DateTime releaseDate, Guid studioId, float price, Studio studio) : base(name)
    {
        Name = name;
        Gender = gender;
        ReleaseDate = releaseDate;
        StudioId = studioId;
        Price = price;
        Studio = studio;
    }
    public void AddStudio(Studio studio) => Studio = studio;
}