using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Core.Extentions;
using UnityEngine;

namespace GameCore.Core.Base.Factory
{
    public class BaseFactory<TType, TBaseFactoryElement>
        where TType : struct
        where TBaseFactoryElement : class 
    {
        protected readonly Dictionary<TType, Type> FactoryElementTypes;
        protected readonly Func<Type, object[], TBaseFactoryElement> FactoryCreateElementMethod;

        public BaseFactory(Func<Type, object[], TBaseFactoryElement> factoryCreateElementMethod)
        {
            FactoryCreateElementMethod = factoryCreateElementMethod;
            FactoryElementTypes = new Dictionary<TType, Type>();
            
            var typeOfBaseClass = typeof(TBaseFactoryElement);
            var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(e => e.GetTypes());
            var childTypes = allTypes.Where(t => (t.IsSubclassOf(typeOfBaseClass) || typeOfBaseClass.IsAssignableFrom(t)) && t != typeOfBaseClass);
            foreach (var childType in childTypes)
            {
                var factoryAttribute = childType.GetAttributeByInterface<IFactoryElementAttribute<TType>>(true);
                if (factoryAttribute != null && !FactoryElementTypes.ContainsKey(factoryAttribute.Type))
                {
                    FactoryElementTypes.Add(factoryAttribute.Type, childType);
                }
                else
                {
                    Debug.LogWarning("more one element contain key " + factoryAttribute.Type);
                }
            }
        }

        public TBaseFactoryElement CreateInstance(TType type, params object[] ctorParam)
        {
            var elementType = default(Type);
            if (FactoryElementTypes.TryGetValue(type, out elementType))
            {
                return FactoryCreateElementMethod.SafeInvoke(elementType,ctorParam);
            }
            return null;
        }
    }
}