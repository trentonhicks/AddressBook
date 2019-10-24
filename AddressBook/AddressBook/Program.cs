using System;

namespace AddressBook
{
    class Program
    {
        static void Main(string[] args)
        {
            var collect = new Collect();
            var sql = new SQL();

            Menu(collect, sql);
        }
        //This is the first menu that is presented to you
        public static void Menu(Collect collect, SQL sql)
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
                    CreateContact(collect, sql);
                    break;

                case "2":
                    ContactListSelection();
                    break;
            }
        }
        //Menu screen when 1.Create Contact has been selected
        public static void CreateContact(Collect collect, SQL sql)
        {

            var contact = new Contact();
            var address = new Address();
            //Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("" +
                "0.=>Back to Menu\n" +
                "1.First Name*:\n" +//change color to blue,Console.ForegroundColor
                "2.Last Name:\n" +
                "3.Phone Number:\n" +
                "4.Email:\n" +
                "5.Address:\n" +
                "\n" +
                "REQUIRED FEILDS*");
            Console.Write("Number selected: ");

            var input = Console.ReadLine();



            switch (input)
            {
                case "0":
                    Menu(collect, sql);
                    break;
                case "1":
                    contact.FirstName = collect.CollectField(fieldName: "First Name", previousValue:contact.FirstName, required: true);
                    break;
                case "2":
                    contact.LastName = collect.CollectField(fieldName: "Last Name", previousValue: contact.LastName, required: false);
                    break;
                case "3":
                    Console.WriteLine("Phone Number");
                    break;
                case "4":
                    Console.WriteLine("Email");
                    break;
                case "5":
                    address.StreetName = collect.CollectField(fieldName: "Street Name", previousValue: address.StreetName, required: false);
                    address.City = collect.CollectField(fieldName: "City", previousValue: address.City, required: false);
                    address.State = collect.CollectField(fieldName: "State", previousValue: address.State, required: false);
                    address.ZipCode = collect.CollectField(fieldName: "ZipCode", previousValue: address.ZipCode, required: false);
                    address.Type = collect.CollectField(fieldName: "Type", previousValue: address.Type, required: false);
                    break;
            }
        }

        //Menu screen when 1.Contact List has been selected
        public static void PhoneNumberScreen()
        {

        }
        public static void ContactListSelection()
        {
            var collect = new Collect();
            var sql = new SQL();
            Console.WriteLine("" +
                "0.=>Back to Menu\n" +
                "1.View/Edit\n" +
                "2.Delete:\n");
            sql.DisplayContactsList();
            var basicInfo = collect.CollectInt($"\nType the ID of the contact you wish to delete: ");


            var input = Console.ReadLine();

            //DisplayContactsList(id)


            Console.Clear();

            switch (input)
            {
                case "0":
                    Menu(collect, sql);
                    break;
                case "1":
                    Console.WriteLine("View/Edit");
                    break;
                case "2":
                    Console.WriteLine("Delete");
                    break;
            }

        }
        public static void ViewEdit(SQL sql)
        {

            Console.Write("" +
                "0.=>Return to previous menu" +
                "\n\nEnter the ID of the contact you wish to View/Edit:");

            var input = Console.ReadLine();
            //Display the list of contacts here 
            //sql.displaycontacts
            //conditional logic when a user selects an id we can use method "sql.id"
            //display id, firstname, lastname
            Console.Clear();

            switch (input)
            {
                case "0":
                    ContactListSelection();
                    break;
                case "1":
                    Console.WriteLine("View/Edit");
                    break;
                case "2":
                    Console.WriteLine("Delete");
                    break;
            }
        }
        public static void DeleteContact()
        {
            var collect = new Collect();

            Console.Write("" +
                "0.=>Return to previous menu" +
                "\n\nEnter the ID of the contact you wish to delete:");
        }
        //ID has been selected this is menu 
        public static void ContactScreens()
        {
            /*display full contact info
            id-fistname-lastname-phonenumber-email-address
             */

            var collect = new Collect();
            SQL sql = new SQL();

            Console.WriteLine("" +
            "0.=>Back to Menu\n" +
            "1.Title\n" +
            "2.First Name*:\n" +
            "3.Last Name:\n" +
            "4.Phone Number:\n" +
            "5.Email:\n" +
            "6.Address:\n" +
            "\n" +
            "REQUIRED FEILDS*");
            Console.Write("Select number you wish to Edit:");

            var input = Console.ReadLine();

            Console.Clear();

            switch (input)
            {
                case "0":
                    Menu(collect, sql);
                    break;
                case "1":
                    Console.WriteLine("Title/Edit");
                    break;
                case "2":
                    Console.WriteLine("First Name/Edit");
                    break;
                case "3":
                    Console.WriteLine("Last Name/Edit");
                    break;
                case "4":

                    Console.WriteLine("Phone Number/Edit");
                    break;
                case "5":
                    Console.WriteLine("Email/Edit");
                    break;
                case "6":
                    Console.WriteLine("Address/Edit");
                    break;
            }

        }

    }
}
