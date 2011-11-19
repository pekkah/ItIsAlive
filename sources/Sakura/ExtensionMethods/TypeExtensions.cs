namespace Sakura.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class TypeExtensions
    {
        public static bool HasInterface(this Type type, Type interfaceType)
        {
            var interfaces = type.GetInterfaces();

            if (interfaceType.IsGenericType)
            {
                foreach (var genericInterface in interfaces.Where(itf => itf.IsGenericType))
                {
                    if (genericInterface.GetGenericTypeDefinition() == interfaceType)
                    {
                        return true;
                    }
                }
            }

            return interfaces.Any(interfaceType.IsAssignableFrom);
        }
    }

    public static class DictionaryExtensions
    {
    }
}