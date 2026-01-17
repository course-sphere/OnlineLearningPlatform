namespace Infrastructure.Repositories
{
    public class QuizRepository : GenericRepository<Domain.Entities.Quiz>, Application.IRepositories.IQuizRepository
    {
        public QuizRepository(AppDbContext context) : base(context)
        {
        }
    }
}
