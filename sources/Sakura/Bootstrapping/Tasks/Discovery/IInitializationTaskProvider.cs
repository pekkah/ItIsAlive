namespace Sakura.Bootstrapping.Tasks.Discovery
{
    using System.Collections.Generic;

    using Sakura.Bootstrapping.Tasks.Types;

    public interface IInitializationTaskProvider
    {
        IEnumerable<IInitializationTask> Tasks { get; }
    }
}