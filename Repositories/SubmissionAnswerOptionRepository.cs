using Application.IRepositories;
using Domain.Entities;

namespace Repositories
{
    public class SubmissionAnswerOptionRepository : GenericRepository<SubmissionAnswerOption>, ISubmissionAnswerOptionRepository
    {
        public SubmissionAnswerOptionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
