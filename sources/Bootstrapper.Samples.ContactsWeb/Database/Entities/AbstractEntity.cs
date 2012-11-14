namespace Bootstrapper.Samples.ContactsWeb.Database.Entities
{
    using System;

    public abstract class AbstractEntity
    {
        public virtual Guid Id { get; set; }
    }
}