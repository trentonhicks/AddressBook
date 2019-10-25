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
            while (true)
            {
                Console.WriteLine("1.Create Contact: ");

                Console.WriteLine("2.Contact List: ");

                Console.Write("\nNumber selected: ");

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
        }

        //Menu screen when 1.Create Contact has been selected
        public static void CreateContact(Collect collect, SQL sql)
        {
            while (true)
            {
                //creating variables for adding user input to the classes.
                var contact = new Contact();

                while (true)
                {
                    Console.Clear();

                    //display
                    Console.WriteLine("0.=>Back to Menu");

                    Console.WriteLine($"1.FirstName: {contact.FirstName}");

                    Console.WriteLine($"2.Last Name: {contact.LastName}");

                    Console.Write($"3.Phone Number: ");
                    foreach (var phoneNumbers in contact.PhoneNumbers)
                    {
                        Console.Write("" +
                            $"Number:{phoneNumbers.Number} / Type:{phoneNumbers.Type}");
                    }

                    Console.Write($"\n4.Email: ");
                    foreach (var Emails in contact.Emails)
                    {
                        Console.Write("" +
                            $"Text:{Emails.Text} / Type:{Emails.Type}");
                    }

                    Console.WriteLine($"\n5.Address: ");
                    foreach (var addresses in contact.Addresses)
                    {
                        Console.Write("" +
                            $"-Street Name:{addresses.StreetName}" +
                            $"\n-City:{addresses.City}" +
                            $"\n-State:{addresses.State}" +
                            $"\n-ZipCode:{addresses.ZipCode}\n");
                    }
                    Console.WriteLine("\n6.Save");

                    Console.Write("\nNumber selected: ");

                    var input = Console.ReadLine();

                    //adding input to database once case is declared.
                    switch (input)
                    {
                        case "0":
                            Menu(collect, sql);
                            break;
                        case "1":
                            contact.FirstName = collect.CollectField(fieldName: "First Name", previousValue: contact.FirstName, required: true);
                            break;
                        case "2":
                            contact.LastName = collect.CollectField(fieldName: "Last Name", previousValue: contact.LastName, required: false);
                            break;
                        case "3":
                            var phoneNumber = new PhoneNumber();

                            phoneNumber.Number = collect.CollectField(fieldName: "Number", previousValue: phoneNumber.Number, required: false);
                            phoneNumber.Type = collect.CollectField(fieldName: "Type", previousValue: phoneNumber.Type, required: false);

                            contact.PhoneNumbers.Add(phoneNumber);
                            break;
                        case "4":
                            var email = new Email();

                            email.Text = collect.CollectField(fieldName: "Text", previousValue: email.Text, required: false);
                            email.Type = collect.CollectField(fieldName: "Type", previousValue: email.Type, required: false);

                            contact.Emails.Add(email);
                            break;
                        case "5":
                            var address = new Address();

                            address.StreetName = collect.CollectField(fieldName: "Street Name", previousValue: address.StreetName, required: false);
                            address.City = collect.CollectField(fieldName: "City", previousValue: address.City, required: false);
                            address.State = collect.CollectField(fieldName: "State", previousValue: address.State, required: false);
                            address.ZipCode = collect.CollectField(fieldName: "ZipCode", previousValue: address.ZipCode, required: false);
                            address.Type = collect.CollectField(fieldName: "Type", previousValue: address.Type, required: false);

                            contact.Addresses.Add(address);
                            break;
                        case "6":
                            sql.CreateContact(contact);
                            return;
                    }
                }
            }
        }
        public static void ContactListSelection()
        {
            var collect = new Collect();
            var sql = new SQL();
            sql.DisplayContactsList();

            Console.WriteLine("" +
                "\n0.=>Back to Menu\n" +
                "1.View/Edit\n" +
                "2.Delete:\n");

            Console.Write("Number Selected:");

            var input = Console.ReadLine();


            Console.Clear();

            switch (input)
            {
                case "0":
                    Menu(collect, sql);
                    break;
                case "1":
                    ViewEdit(sql);
                    break;
                case "2":
                    Console.WriteLine("Delete");
                    break;
            }
        }
        public static void ViewEdit(SQL sql)
        {

            while (true)
            {
                sql.DisplayContactsList();

                Console.Write("" +
                    "\n0.=> Return to previous menu" +
                    "\n\nEnter the ID of the contact you wish to View/Edit:");

                var input = Console.ReadLine();

                Console.Clear();

                switch (input)
                {
                    case "0":
                        ContactListSelection();
                        break;
                }
            }
        }
        public static void DeleteContact()
        {
            var collect = new Collect();

            Console.Write("" +
                "0.=>Return to previous menu" +
                "\n\nEnter the ID of the contact you wish to delete:");
        }
    }
}
