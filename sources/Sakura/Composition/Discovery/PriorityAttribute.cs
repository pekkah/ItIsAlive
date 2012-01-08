namespace Sakura.Composition.Discovery
{
    using System;

    public class PriorityAttribute : Attribute, IPriorityMetadata
    {
        public int Priority
        {
            get;
            set;
        }
    }
}