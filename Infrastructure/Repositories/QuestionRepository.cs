namespace Infrastructure.Repositories
{
    public class QuestionRepository : GenericRepository<Domain.Entities.Question>, Application.IRepositories.IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
