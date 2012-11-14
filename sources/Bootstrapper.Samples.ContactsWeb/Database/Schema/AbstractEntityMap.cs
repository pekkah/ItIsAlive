namespace Bootstrapper.Samples.ContactsWeb.Database.Schema
{
    using Bootstrapper.Samples.ContactsWeb.Database.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    public abstract class AbstractEntityMap<T> : ClassMapping<T>
        where T : AbstractEntity
    {
        protected AbstractEntityMap()
        {
            this.Id(x => x.Id, map => map.Generator(Generators.GuidComb));
        }
    }
}