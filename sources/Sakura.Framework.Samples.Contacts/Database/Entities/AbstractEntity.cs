namespace Sakura.Framework.Samples.Contacts.Database.Entities
{
    using System;

    public abstract class AbstractEntity
    {
        public virtual Guid Id { get; set; }
    }
}