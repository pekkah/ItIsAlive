namespace ItIsAlive.ExtensionMethods
{
    using System;
    using System.Linq;

    public static class TypeExtensions
    {
        public static bool HasInterface(this Type type, Type interfaceType)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (interfaceType == null)
            {
                throw new ArgumentNullException("interfaceType");
            }

            Type[] interfaces = type.GetInterfaces();

            if (interfaceType.IsGenericType)
            {
                if (
                    interfaces.Where(itf => itf.IsGenericType).Any(
                        genericInterface => genericInterface.GetGenericTypeDefinition() == interfaceType))
                {
                    return true;
                }
            }

            return interfaces.Any(interfaceType.IsAssignableFrom);
        }
    }
}