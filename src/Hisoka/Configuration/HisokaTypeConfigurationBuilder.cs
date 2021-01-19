using System;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;


namespace Hisoka.Configuration
{
    public class HisokaTypeConfigurationBuilder<TEntity>
    {
        internal Dictionary<string, HisokaPropertyMetadata> HisokaPropertyMap { get; }

        public HisokaTypeConfigurationBuilder() 
        {
            HisokaPropertyMap = new Dictionary<string, HisokaPropertyMetadata>();
        }

        public HisokaPropertyMetadata Property(Expression<Func<TEntity, object>> expression, string alias) 
        {
            var property = new HisokaPropertyMetadata((PropertyInfo)expression.ToMemberInfo<TEntity>());
            property.HasAlias(alias);

            HisokaPropertyMap.Add(alias.ToLower(), property);
            return property;
        }

        public HisokaPropertyMetadata Property(Expression<Func<TEntity, object>> expression) 
        {
            var property = new HisokaPropertyMetadata((PropertyInfo)expression.ToMemberInfo<TEntity>());
            HisokaPropertyMap.Add(property.Alias.ToLower(), property);

            return property;
        }
    }
}