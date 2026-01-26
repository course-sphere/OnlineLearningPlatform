using Application.IRepositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class ModuleRepository : GenericRepository<Module>, IModuleRepository
    {
        public ModuleRepository(AppDbContext context) : base(context)
        {
        }
    }
}
