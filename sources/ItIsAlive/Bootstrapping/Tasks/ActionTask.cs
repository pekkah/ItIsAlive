namespace ItIsAlive.Bootstrapping.Tasks
{
    using System;

    public class ActionTask : IInitializationTask
    {
        private readonly Action<InitializationTaskContext> action;

        public ActionTask(Action<InitializationTaskContext> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.action = action;
        }

        public void Execute(InitializationTaskContext context)
        {
            action(context);
        }
    }
}