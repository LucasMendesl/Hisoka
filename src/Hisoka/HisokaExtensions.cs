using Hisoka;
using System.Reflection;
using System.Collections;

namespace System.Linq
{
    /// <summary>
    /// Classe responsável por prover métodos de extensão para as coleções IQueryable
    /// </summary>
    public static partial class HisokaExtensions
    {        
        internal static bool IsEnumerable<TEntity>(this string property)
        {
            var prop = typeof(TEntity).GetProperty(property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (prop == null)
                throw new HisokaException(string.Format("Property '{0}' is not a member of the taget entity.", property));

            return prop.PropertyType.IsEnumerableType();
        }

        internal static bool IsEnumerableType(this Type type)
        {
            return type != typeof(string) && (typeof(IEnumerable).IsAssignableFrom(type));
        }
    }
}
