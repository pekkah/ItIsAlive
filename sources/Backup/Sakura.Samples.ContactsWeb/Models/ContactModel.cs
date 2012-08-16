namespace Sakura.Samples.ContactsWeb.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ContactModel
    {
        public Guid Id
        {
            get;
            set;
        }

        [Required]
        [Display(Name = "Name")]
        public string Name
        {
            get;
            set;
        }
    }
}