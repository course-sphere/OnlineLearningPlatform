namespace Infrastructure.Repositories
{
    public class QuizAttemptRepository : GenericRepository<Domain.Entities.QuizAttempt>, Application.IRepositories.IQuizAttemptRepository
    {
        public QuizAttemptRepository(AppDbContext context) : base(context)
        {
        }
    }
}
