
var contactManager = (function () {
    // var age = 40;

    function loadContacts(success) {
        $.getJSON("api/contacts")
            .success(success);
    }

    function addContact(contact, success) {
        $.post("api/contacts/add", contact)
            .success(success);
    }

    function removeContact(contact) {
        // todo (pekka) remove contact
    }

    return {
        loadAll: loadContacts,
        remove: removeContact,
        add: addContact
    };
} ());