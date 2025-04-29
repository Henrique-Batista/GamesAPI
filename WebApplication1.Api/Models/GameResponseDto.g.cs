using System;
using WebApplication1.API.Domain.Enums;

namespace WebApplication1.API.Domain
{
    public partial class GameResponseDto
    {
        public DateTime ReleaseDate { get; set; }
        public GameGender Gender { get; set; }
        public Guid StudioId { get; set; }
        public float Price { get; set; }
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}