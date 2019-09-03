using System.Linq;

namespace Hisoka
{
    public interface IQuery<T>
        where T : class
    {
        IQueryable<T> Apply(IQueryable<T> source);
    }
}
