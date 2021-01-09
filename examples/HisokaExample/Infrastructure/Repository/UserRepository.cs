using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hisoka;
using HisokaExample.Infrastructure.Context;
using HisokaExample.Model;
using Microsoft.EntityFrameworkCore;

namespace HisokaExample.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly HisokaContext _context;

        public UserRepository(HisokaContext context)
        {
            _context = context;
        }

        public async Task<List<object>> GetUsers(ResourceQueryFilter query)
        {
            var users = await _context.Users
                    .Include(x => x.UserRoles)
                        .ThenInclude(x => x.Role)
                    .ProjectedQuery(query)
                    .ToListAsync<object>();

            return users;
        }
    }
}