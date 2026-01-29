using Application.IRepositories;
using Domain.Entities;

namespace Repositories
{
    public class GradedAttemptRepository : GenericRepository<GradedAttempt>, IGradedAttemptRepository
    {
        public GradedAttemptRepository(AppDbContext context) : base(context)
        {
        }
    }
}
