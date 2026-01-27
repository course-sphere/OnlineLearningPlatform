using Application.IRepositories;
using Domain.Entities;
using Infrastructure;

namespace Infrastructure.Repositories
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        private readonly AppDbContext _context;

        public EnrollmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Enrollment> GetQueryable()
        {
            return _context.Enrollments;
        }
    }
}