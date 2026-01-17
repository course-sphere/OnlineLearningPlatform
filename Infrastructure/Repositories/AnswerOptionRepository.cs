namespace Infrastructure.Repositories
{
    public class AnswerOptionRepository : GenericRepository<Domain.Entities.AnswerOption>, Application.IRepositories.IAnswerOptionRepository
    {
        public AnswerOptionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
