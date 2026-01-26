
namespace Domain.Entities
{
    public class WalletTransaction
    {
        public Guid WalletTransactionId { get; set; }
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public WalletTransactionType TransactionType { get; set; }
        public string? Description { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Wallet? Wallet { get; set; }

       
    }
    public enum WalletTransactionType
    {
        CourseSale, 
        Refund,       
        Withdraw,     
        Adjustment
    }
}
