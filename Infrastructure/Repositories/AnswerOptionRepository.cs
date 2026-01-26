using Application.IRepositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class AnswerOptionRepository : GenericRepository<AnswerOption>, IAnswerOptionRepository
    {
        public AnswerOptionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
