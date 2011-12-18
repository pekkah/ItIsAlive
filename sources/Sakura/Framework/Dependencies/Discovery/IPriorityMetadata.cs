namespace Sakura.Framework.Dependencies.Discovery
{
    using System;

    public interface IPriorityMetadata
    {
        int Priority
        {
            get;
        }
    }

    public class PriorityAttribute : Attribute, IPriorityMetadata
    {
        public int Priority
        {
            get;
            set;
        }
    }
}