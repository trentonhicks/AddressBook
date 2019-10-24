using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook
{
    class PhoneNumber
    {
        // ID
        private int _id;
        public int ID
        {
            get => _id != 0 ? _id : -1;
            set => _id = value;
        }

        // Number
        private string _number;
        public string Number
        {
            get => _number != "" ? _number : "NULL";
            set => _number = value;
        }

        // Type
        private string _type;
        public string Type
        {
            get => _type != "" ? _type : "NULL";
            set => _type = value;
        }

    }
}
