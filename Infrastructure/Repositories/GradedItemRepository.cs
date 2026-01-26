using Application.IRepositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class GradedItemRepository : GenericRepository<GradedItem>, IGradedItemRepository
    {
        public GradedItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
