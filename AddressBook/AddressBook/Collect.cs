using System;
using System.Collections.Generic;
using System.Text;
​
namespace AddressBook
{
    class Collect
    {
        /// <summary>
        /// Passes in information from the user and re
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string CollectField(string fieldName, bool required)
        {
            Console.Clear();
            var userInput = "";
            if (required)
            {
                while (true)
                {
                    //Check to see if input is added to the required field.//
                    Console.Write($"{fieldName}: ");
                    userInput = Console.ReadLine();
                    if (userInput == "")
                    {
                        Console.WriteLine($"{fieldName} is required");
                    }
                    //If input is dectected in the required field than break the while loop.//
                    else
                    {
                        break;
                    }
​
                }
            }
            else
            {
                //Collect info for user if not required.//
                Console.Write($"{fieldName}: ");
                userInput = Console.ReadLine();
            }
            return userInput;
        }
​
        public Address CollectAddress()
        {
            var address = new Address();
            address.StreetName = CollectField(fieldName: "Street Name", required: false);
            address.City = CollectField(fieldName: "City", required: false);
            address.State = CollectField(fieldName: "State", required: false);
            address.ZipCode = CollectField(fieldName: "Zip", required: false);
            address.Type = CollectField(fieldName: "Type", required: false);
            return address;
        }

        public int CollectInt(int fieldNumber, string value)
        {
            for (var i = 0; i < value.Length; i++)
                if (value == null)
                {
                    return 0;
​
            }

                else
                {
                    int valueInt = value[i];
                }
            return value;
        }
        public int CollectPhoneNumber()
        {
            var phonenumber = CollectInt(fieldNumber: "PhoneNumber" : false);
            var type = CollectInt(required: false);
            return phonenumber;

        }
​
        public string CollectEmail()
        {
            var email = CollectField(fieldName: "Email", required: false);
            var type = CollectField(fieldName: "Type", required: false);
            return email;
        }
​
        
    }
}