
namespace Domain.Entities
{
    public class Wallet
    {
        public Guid WalletId { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public decimal PendingBalance { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalWithdrawn { get; set; }
        public WalletStatus Status { get; set; } = WalletStatus.Active;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User? User { get; set; }
    }
    public enum WalletStatus
    {
        Active,
        Suspended
    }
}
