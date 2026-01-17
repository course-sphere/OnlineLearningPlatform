using Application.IRepositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
