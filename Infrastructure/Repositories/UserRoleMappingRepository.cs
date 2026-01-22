using Application.IRepositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class UserRoleMappingRepository : GenericRepository<UserRoleMapping>, IUserRoleMappingRepository
    {
        public UserRoleMappingRepository(AppDbContext context) : base(context)
        {
        }
    }
}
