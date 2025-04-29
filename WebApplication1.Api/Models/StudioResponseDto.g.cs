using System;

namespace WebApplication1.API.Domain
{
    public partial class StudioResponseDto
    {
        public string Country { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}