using Application.IRepositories;
using Domain.Entities;

namespace Repositories
{
    public class QuestionSubmissionRepository : GenericRepository<QuestionSubmission>, IQuestionSubmissionRepository
    {
        public QuestionSubmissionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
