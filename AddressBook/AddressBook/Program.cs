using System;

namespace AddressBook
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }
        //This is the first menu that is presented to you
        public static void Menu()
        {
            Console.WriteLine("" +
                "1.Create Contact\n" +
                "2.Contact list\n");

            Console.Write("Number selected: ");


            var input = Console.ReadLine();

            Console.Clear();

            switch (input)
            {
                case "1":
                    CreateContact();
                    break;

                case "2":
                    ContactListSelection();
                    break;
            }
        }
        //Menu screen when 1.Create Contact has been selected
        public static void CreateContact()
        {

            //Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("" +
                "0.=>Back to Menu\n" +
                "1.Title\n" +
                "2.First Name*:\n"+//change color to blue,Console.ForegroundColor
                "3.Last Name:\n" +
                "4.Phone Number:\n" +
                "5.Email:\n" +
                "6.Address:\n" +
                "\n" +
                "REQUIRED FEILDS*");
            Console.Write("Number selected: ");

            var input = Console.ReadLine();

            Console.Clear();

            switch (input)
            {
                case "0":
                    Menu();
                    break;
                case "1":
                    Console.WriteLine("Title");
                    break;
                case "2":
                    Console.WriteLine("First Name");
                    break;
                case "3":
                    Console.WriteLine("Last Name");
                    break;
                case "4":
                    Console.WriteLine("Phone Number");
                    break;
                case "5":
                    Console.WriteLine("Email");
                    break;
                case "6":
                    Console.WriteLine("Address");
                    break;
            }

        }
        //Menu screen when 1.Contact List has been selected
        public static void ContactListSelection()
        {
            Console.WriteLine("" +
                "0.=>Back to Menu\n" +
                "1.View/Edit\n" +
                "2.Delete:\n");

            Console.Write("Number selected: ");

            var input = Console.ReadLine();
            //Display the list of contacts here 
            //sql.displaycontacts
            //conditional logic when a user selects an id we can use method "sql.id"
            //display id, firstname, lastname
            Console.Clear();

            switch (input)
            {
                case "0":
                    Menu();
                    break;
                case "1":
                    Console.WriteLine("View/Edit");
                    break;
                case "2":
                    Console.WriteLine("Delete");
                    break;
            }

        }
        public static void ViewEdit()
        {

        }
    }
}

