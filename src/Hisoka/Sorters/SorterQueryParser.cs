using System.Linq;
using Hisoka.Configuration;

namespace Hisoka
{
    class SorterQueryParser<T>
        where T : class
    {
        public string ParseExpression(Sort sort) 
        {
            var propArray = sort.PropertyName.Split('.');

            if (propArray.Length > 2)
            {
                throw new HisokaException("Unsupporter array argument lenght.");
            }

            if (propArray[0].IsEnumerable<T>()) 
            {
                throw new HisokaException($"cannot sort collection property '{propArray[0]}'");
            }

            var newProperty = new string[propArray.Length];

            for (int i = 0; i < propArray.Length; i++)
            {
                var metadata = HisokaConfiguration.GetPropertyMetadataFromCache(typeof(T), propArray[i]);

                if (metadata != null) 
                {
                    newProperty[i] = metadata.CurrentProperty.Name;
                    continue;                    
                }

                metadata = HisokaConfiguration.FindPropertyMetadataInCache(propArray[i]);

                if (metadata == null) 
                {
                    throw new HisokaException(string.Format("Property '{0}' is not a member of the taget entity.", propArray[i]));
                }

                newProperty[i] = metadata.CurrentProperty.Name;
            }

            return string.Join(".", newProperty);
        }
    }
}