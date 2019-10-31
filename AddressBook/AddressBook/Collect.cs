using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook
{
    class Collect
    {
        /// <summary>
        /// Passes in information from the user and strores it.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// 
        
        public string CollectField(string fieldName, string previousValue, bool required)
        {
            Console.Clear();
            var userInput = "";
            if (required)
            {
                while (true)
                {
                    //Check to see if input is added to the required field.//
                    Console.Write($"Press 0 to go back:\n\n{fieldName}: ");
                    userInput = Console.ReadLine();
                    if (userInput == "0")
                    {
                        return previousValue;
                    }
                    else if (userInput == "")
                    {


                        Console.WriteLine($"{fieldName} is required");
                    }
                    //If input is dectected in the required field than break the while loop.//
                    else
                    {
                        break;
                    }

                }
            }
            else
            {
                //Collect info for user if not required.//
                Console.Write($"{fieldName}: ");
                userInput = Console.ReadLine();
                if (userInput == "0")
                {
                    return previousValue;
                }
            }
            return userInput;
        }

        //public string CollectAddress(string fieldName, string previousValue, bool required)
        //{
        //    Console.Clear();
        //    var userInput = "";

        //    if(userInput == "0")
        //}

        
        
        //Collects int and only int for ID//
        public int CollectID(string msg)
        {
            Console.Write($"{msg}: ");
            //If an int return user input//
            while (true)
            {
                int number;
                var Input = Console.ReadLine();
                bool isNumber = int.TryParse(Input, out number);
                if (isNumber)
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("Wrong Input! Int only!");
                }
            }
        }
    }
}
