using Application.IRepositories;

namespace Application
{
    public interface IUnitOfWork
    {
        public IUserRepository Users { get; }
        Task SaveChangeAsync();
    }
}
