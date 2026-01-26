namespace Infrastructure.Repositories
{
    public class SubmissionRepository : GenericRepository<Domain.Entities.Submission>, Application.IRepositories.ISubmissionRepository
    {
        public SubmissionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
