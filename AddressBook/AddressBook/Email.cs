using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook
{
    class Email
    {
        // ID
        private int _id;
        public int ID
        {
            get => _id != 0 ? _id : -1;
            set => _id = value;
        }

        // Text
        private string _text;
        public string Text
        {
            get => _text != "" ? _text : "NULL";
            set => _text = value;
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
