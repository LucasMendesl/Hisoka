using Hisoka;
using System.Threading.Tasks;


namespace HisokaExample.Infrastructure.Repository
{
    public interface IUserRepository
    {
         Task<IPagedList<object>> GetUsers(ResourceQueryFilter query);

         Task<IPagedList<object>> GetUserRoles(ResourceQueryFilter query);
    }
}