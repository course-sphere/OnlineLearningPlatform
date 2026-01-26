using Application.IRepositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class LessonResourceRepository : GenericRepository<LessonResource>, ILessonResourceRepository
    {
        public LessonResourceRepository(AppDbContext context) : base(context)
        {
        }
    }
}
