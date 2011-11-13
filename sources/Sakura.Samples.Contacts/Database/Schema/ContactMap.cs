namespace Sakura.Samples.Contacts.Database.Schema
{
    using Sakura.Samples.Contacts.Database.Entities;

    public class ContactMap : AbstractEntityMap<Contact>
    {
        public ContactMap()
        {
            this.Property(x => x.Name, pm => pm.Length(100));
        }
    }
}