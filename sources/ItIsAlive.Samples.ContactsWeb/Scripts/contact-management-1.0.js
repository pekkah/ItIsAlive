
var contactManager = (function () {
    // var age = 40;

    function loadContacts(success) {
        $.getJSON("api/contacts")
            .success(success);
    }

    function addContact(contact, success) {
        $.ajax({
            type: "POST",
            url: "api/contacts",
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify(contact)
        }).done(function (msg) {
            success();
        });
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