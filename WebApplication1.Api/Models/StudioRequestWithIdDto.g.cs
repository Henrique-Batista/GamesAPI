using System;

namespace WebApplication1.API.Domain
{
    public partial class StudioRequestWithIdDto
    {
        public string Country { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}