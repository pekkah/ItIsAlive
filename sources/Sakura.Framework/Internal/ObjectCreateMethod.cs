namespace Fugu.Framework.Internal
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    internal class ObjectCreateMethod
    {
        private MethodInvoker methodHandler = null;

        public ObjectCreateMethod(Type type)
        {
            this.CreateMethod(type.GetConstructor(Type.EmptyTypes));
        }

        public ObjectCreateMethod(ConstructorInfo target)
        {
            this.CreateMethod(target);
        }

        private delegate object MethodInvoker();

        public T CreateInstance<T>() where T : class
        {
            return this.methodHandler() as T;
        }

        private void CreateMethod(ConstructorInfo target)
        {
            var dynamic = new DynamicMethod(string.Empty, typeof(object), new Type[0], target.DeclaringType);

            ILGenerator il = dynamic.GetILGenerator();
            il.DeclareLocal(target.DeclaringType);
            il.Emit(OpCodes.Newobj, target);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            this.methodHandler = (MethodInvoker)dynamic.CreateDelegate(typeof(MethodInvoker));
        }
    }
}