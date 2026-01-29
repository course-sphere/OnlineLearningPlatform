namespace Repositories
{
    public class PaymentRepository : GenericRepository<Domain.Entities.Payment>, Application.IRepositories.IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
