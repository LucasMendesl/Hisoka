using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using Hisoka.Configuration;
using System.Linq.Expressions;
using System.Collections.Concurrent;

namespace Hisoka
{
    static class RelfectionExtensions
    {
        internal static ConcurrentDictionary<Type, bool> InternalPropertyCache = new ConcurrentDictionary<Type, bool>();    

        internal static bool IsEnumerable<TEntity>(this string property)
        {
            var currentType = typeof(TEntity);

            if (!InternalPropertyCache.TryGetValue(currentType, out var isEnumerable)) 
            {
                var propertyMetadata = HisokaConfiguration.GetPropertyMetadataFromCache(currentType, property);
                var prop = propertyMetadata.CurrentProperty;

                if (prop == null)
                    throw new HisokaException(string.Format("Property '{0}' is not a member of the taget entity.", property));

                isEnumerable = prop.PropertyType.IsEnumerableType();
                InternalPropertyCache.TryAdd(currentType, isEnumerable);
            }

            return isEnumerable;
        }

        internal static bool IsEnumerableType(this Type type)
        {
            return type != typeof(string) && (typeof(IEnumerable).IsAssignableFrom(type));
        }

        internal static MemberInfo ToMemberInfo<TEntity>(this LambdaExpression expression) 
        {
            Expression expr = expression;
            while (true)
            {
                switch (expr.NodeType)
                {
                    case ExpressionType.Lambda:
                        expr = ((LambdaExpression)expr).Body;
                        break;

                    case ExpressionType.Convert:
                        expr = ((UnaryExpression)expr).Operand;
                        break;

                    case ExpressionType.MemberAccess:
                        var memberExpression = (MemberExpression)expr;
                        var member = memberExpression.Member;
                        Type paramType;

                        while (memberExpression != null)
                        {
                            paramType = memberExpression.Type;

                            var baseMember = paramType.GetMembers().FirstOrDefault(m => m.Name == member.Name);
                            if (baseMember != null)
                            {
                                if (baseMember is PropertyInfo baseProperty && member is PropertyInfo property)
                                {
                                    if (baseProperty.DeclaringType == property.DeclaringType &&
                                        baseProperty.PropertyType != Nullable.GetUnderlyingType(property.PropertyType))
                                    {
                                        return baseMember;
                                    }
                                }
                                else
                                {
                                    return baseMember;
                                }
                            }

                            memberExpression = memberExpression.Expression as MemberExpression;
                        }

                        paramType = expression.Parameters[0].Type;
                        return paramType.GetMember(member.Name)[0];

                    default:
                        return null;
                }
            }
        }
    }
}