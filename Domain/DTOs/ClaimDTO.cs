using Domain.Entities;

namespace Domain.DTOs
{
    public class ClaimDTO
    {
        public Guid UserId { get; set; }
        public Role Role { get; set; }
    }
}
