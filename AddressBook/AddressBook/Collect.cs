using System;
using System.Collections.Generic;
using System.Text;

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
            var userInput = "";
            if (required)
            {
                while (true)
                {
                    Console.ReadLine();
                    if (userInput == "")
                    {
                        Console.WriteLine($"{fieldName} is required");
                    }
                    //If input is dectected in the required field than break the while loop. 
                    else
                    {
                        break;
                    }

                }
            }
            else
            {

            }
            return userInput;
        }


    }
}
