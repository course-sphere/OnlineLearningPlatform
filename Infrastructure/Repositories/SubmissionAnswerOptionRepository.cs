using Application.IRepositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class SubmissionAnswerOptionRepository : GenericRepository<SubmissionAnswerOption>, ISubmissionAnswerOptionRepository
    {
        public SubmissionAnswerOptionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
