namespace ItIsAlive.Samples.ContactsWeb.Database.Entities
{
    using System.Collections.Generic;

    public class User : AbstractEntity
    {
        private readonly IList<Contact> contacts;

        public User()
        {
            this.contacts = new List<Contact>();
        }

        public virtual IEnumerable<Contact> Contacts
        {
            get
            {
                return this.contacts;
            }
        }

        public virtual string Email { get; set; }

        public virtual string Name { get; set; }

        public virtual string Password { get; set; }

        public virtual void AddContact(string name)
        {
            this.contacts.Add(new Contact() { Name = name });
        }
    }
}