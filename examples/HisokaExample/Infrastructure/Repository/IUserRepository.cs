using System.Collections.Generic;
using System.Threading.Tasks;
using Hisoka;

namespace HisokaExample.Infrastructure.Repository
{
    public interface IUserRepository
    {
         Task<List<object>> GetUsers(ResourceQueryFilter query);
    }
}