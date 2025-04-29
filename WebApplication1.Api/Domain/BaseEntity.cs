using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.API.Domain;

public abstract class BaseEntity
{

    [Key]
    public Guid Id { get; init; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; init; }
    
    public DateTime UpdatedAt
    {
        get => _updatedAt;
        init => _updatedAt = value;
    }
    private DateTime _updatedAt;
    
    public BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Id = Guid.NewGuid();
        Name = string.Empty;
    }
    public BaseEntity(string name)
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Id = Guid.NewGuid();
        Name = name;
    }

    public void Update() => _updatedAt = DateTime.UtcNow;
}