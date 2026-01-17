using Application;
using Application.IRepositories;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;

        public IUserRepository Users { get; set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
        }

        public async Task SaveChangeAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(message: ex.Message);
            }
        }
    }
}
