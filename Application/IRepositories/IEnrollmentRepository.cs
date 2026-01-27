using Domain.Entities;
using System.Linq;

namespace Application.IRepositories
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        IQueryable<Enrollment> GetQueryable();
    }
}
