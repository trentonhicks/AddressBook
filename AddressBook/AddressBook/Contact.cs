using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook
{
    class Contact
    {
        // ID
        private int _id;
        public int ID
        {
            get => _id != 0 ? _id : -1;
            set => _id = value;
        }

        // First Name
        private string _firstName;
        public string FirstName
        {
            get => _firstName != "" ? _firstName : "NULL";
            set => _firstName = value;
        }

        // Last Name
        private string _lastName;
        public string LastName
        {
            get => _lastName != "" ? _lastName : "NULL";
            set => _lastName = value;
        }
    }
}
