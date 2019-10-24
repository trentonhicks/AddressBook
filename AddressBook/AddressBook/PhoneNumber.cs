using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook
{
    class PhoneNumber
    {
        private string _number;
        public string Number
        {
            get => _number != "" ? _number : "NULL";
            set => _number = value;
        }

        private string _type;
        public string Type
        {
            get => _type != "" ? _type : "NULL";
            set => _type = value;
        }

    }
}
