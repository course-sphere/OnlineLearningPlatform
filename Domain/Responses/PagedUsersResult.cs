using Domain.Entities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Responses
{
    public class PagedUsersResult
    {
        public List<User> Users { get; set; } = new();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}