using Hisoka;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HisokaExample.Infrastructure.Context;


namespace HisokaExample.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly HisokaContext _context;

        public UserRepository(HisokaContext context)
        {
            _context = context;
        }

        public async Task<IPagedList<object>> GetUserRoles(ResourceQueryFilter query)
        {
            var users = await _context.UserRoles
                .Include(x => x.User)
                .Include(x => x.Role)
                .ProjectedQuery(query)
                .ToPagedListAsync<object>(query.Paginate);

            return users;
        }

        public async Task<IPagedList<object>> GetUsers(ResourceQueryFilter query)
        {
            var users = await _context.Users
                    .Include(x => x.UserRoles)
                        .ThenInclude(x => x.Role)
                    .ProjectedQuery(query)
                    .ToPagedListAsync<object>(query.Paginate);

            return users;
        }
    }
}