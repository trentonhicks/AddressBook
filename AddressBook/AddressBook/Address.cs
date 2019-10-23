﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook
{
    class Address
    {
        // Street Name
        private string _streetName;
        public string StreetName
        {
            get => _streetName != "" ? _streetName : "NULL";
            set => _streetName = value;
        }

        // City
        private string _city;
        public string City
        {
            get => _city != "" ? _city : "NULL";
            set => _city = value;
        }

        // State
        private string _state;
        public string State
        {
            get => _state != "" ? _state : "NULL";
            set => _state = value;
        }

        // ZipCode
        private string _zipCode;
        public string ZipCode
        {
            get => _zipCode != "" ? _zipCode : "NULL";
            set => _zipCode = value;
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
