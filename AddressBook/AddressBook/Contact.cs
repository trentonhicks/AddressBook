using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook
{
    class Contact
    {
        // ID
        public int ID { get; set; }

        // First Name
        private string _firstName;
        public string FirstName
        {
            get => _firstName != "" ? _firstName : "No name";
            set => _firstName = value;
        }

        // Last Name
        private string _lastName;
        public string LastName
        {
            get => _lastName != "" ? _lastName : "NULL";
            set => _lastName = value;
        }

        // Addresses
        public List<Address> Addresses { get; set; } = new List<Address>();
        public List<Email> Emails { get; set; } = new List<Email>();
        public List<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
    }
}
