using System;
using System.Reflection;

namespace Hisoka.Configuration
{
    public class HisokaPropertyMetadata
    {
        public string Alias { get; private set; }
        public bool AllowProjection { get; private set; }
        public bool AllowFilter { get; private set; }
        public PropertyInfo CurrentProperty { get; }

        internal HisokaPropertyMetadata(PropertyInfo property)
        {
            AllowFilter = true;
            AllowProjection = true;
            CurrentProperty = property;
            Alias = property.Name;
        }

        internal HisokaPropertyMetadata HasAlias(string alias) 
        {
            Alias = alias.ToLower();
            return this;
        }

        public HisokaPropertyMetadata DenyFilter() 
        {
            AllowFilter = false;
            return this;
        }

        public HisokaPropertyMetadata DenyProjection() 
        {
            AllowProjection = false;
            return this;
        }
    }
}