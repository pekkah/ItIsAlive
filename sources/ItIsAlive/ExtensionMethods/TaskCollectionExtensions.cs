namespace ItIsAlive.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using Tasks;

    public static class TaskCollectionExtensions
    {
        public static void Add(this ICollection<IInitializationTask> tasks, Action<InitializationTaskContext> task)
        {
            tasks.Add(new ActionTask(task));
        }
    }
}