using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook
{
    class Email
    {
        private string _text;
        public string Text
        {
            get => _text != "" ? _text : "NULL";
            set => _text = value;
        }

        private string _type;
        public string Type
        {
            get => _type != "" ? _type : "NULL";
            set => _type = value;
        }
    }
}
