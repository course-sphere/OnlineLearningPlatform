using Application.IRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserLessonProgressRepository : GenericRepository<UserLessonProgress>, IUserLessonProgressRepository
    {
        public UserLessonProgressRepository(AppDbContext context) : base(context)
        {
        }
    }
}
