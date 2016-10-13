using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Core.Extentions
{
    public static class TypeExtention
    {
        public static TAttribute[] GetAttributes<TAttribute>(this Type type,bool inherit = false) where TAttribute : Attribute
        {
            return Attribute.GetCustomAttributes(type,typeof(TAttribute), inherit).Cast<TAttribute>().ToArray();
        }

        public static TAttribute GetAttribute<TAttribute>(this Type type, bool inherit = false) where TAttribute : Attribute
        {
            var attributes = Attribute.GetCustomAttributes(type,typeof(TAttribute), inherit);
            if (attributes.Length > 0)
                return (TAttribute)attributes[0];
            return null;
        }


        public static TInterface[] GetAttributesByInterface<TInterface>(this Type type, bool inherit = false) where TInterface : class
        {
            var attributes = Attribute.GetCustomAttributes(type, typeof(Attribute), inherit);
            var result = new List<TInterface>();
            if (attributes.Length > 0)
            {
                var interfaceType = typeof(TInterface);
                for(int i = 0; i<attributes.Length;i++)
                {
                    var attribute = attributes[i];
                    var attributeType = attribute.GetType();
                    if (interfaceType.IsAssignableFrom(attributeType))
                    {
                        result.Add((TInterface)(object)attribute);
                    }
                }
            }
            return result.ToArray();
        }

        public static TInterface GetAttributeByInterface<TInterface>(this Type type, bool inherit = false) where TInterface : class
        {
            var attributes = Attribute.GetCustomAttributes(type, typeof (Attribute), inherit);
            if (attributes.Length > 0)
            {
                var interfaceType = typeof(TInterface);
                for (int i = 0; i < attributes.Length; i++)
                {
                    var attribute = attributes[i];
                    var attributeType = attribute.GetType();
                    if (interfaceType.IsAssignableFrom(attributeType))
                    {
                        return attribute as TInterface;
                    }
                }
            }
            return default(TInterface);
        }
    }
}
