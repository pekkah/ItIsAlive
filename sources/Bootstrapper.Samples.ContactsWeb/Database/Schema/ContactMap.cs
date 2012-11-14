namespace Bootstrapper.Samples.ContactsWeb.Database.Schema
{
    using Bootstrapper.Samples.ContactsWeb.Database.Entities;

    public class ContactMap : AbstractEntityMap<Contact>
    {
        public ContactMap()
        {
            this.Property(x => x.Name, pm => pm.Length(100));
        }
    }
}