using System;
using System.Collections.Generic;

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
                    EditContact(collect, sql);
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

        public static void EditContact(Collect collect, SQL sql)
        {
            sql.DisplayContactsList();
            var ID = collect.CollectID("Enter id of contact you'd like to edit");

            // Get contact
            var contact = sql.GetContact(ID);

            // Check if contact exists
            if (contact != null)
            {
                // Edit contact
                while (true)
                {
                    Console.Clear();

                    //display
                    Console.WriteLine("0.=>Back to Menu");
                    Console.WriteLine($"1.FirstName: {contact.FirstName}");
                    Console.WriteLine($"2.Last Name: {contact.LastName}");
                    Console.Write($"3.Phone Numbers: {contact.FirstName} has {contact.PhoneNumbers.Count} number(s)");
                    Console.Write($"\n4.Emails: {contact.FirstName} has {contact.Emails.Count} email(s)");
                    Console.WriteLine($"\n5.Addresses: {contact.FirstName} has {contact.Addresses.Count} address(es)");

                    var input = Console.ReadLine();

                    //adding input to database once case is declared.
                    switch (input)
                    {
                        case "0":
                            Menu(collect, sql);
                            break;
                        case "1":
                            contact.FirstName = collect.CollectField(fieldName: "First Name", previousValue: contact.FirstName, required: true);
                            sql.UpdateFirstName(contact.ID, contact.FirstName);
                            break;
                        case "2":
                            contact.LastName = collect.CollectField(fieldName: "Last Name", previousValue: contact.LastName, required: false);
                            sql.UpdateLastName(contact.ID, contact.LastName);
                            break;
                        case "3":
                            EditPhoneNumbers(collect, sql, contact);
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

                    }
                }
            }

            // Contact doesn't exist
            else
            {
                
            }
        }
        public static void EditPhoneNumbers(Collect collect, SQL sql, Contact contact)
        {
            Console.WriteLine("0. => Go back");
            Console.WriteLine("add. add a new phone number");

            sql.DisplayPhoneNumbers(contact);

            Console.WriteLine("Enter the ID of the number you wish you wish to edit");

            var input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    return;
                case "add":
                    // Store new numbers in a list
                    var phoneNumbers = new List<PhoneNumber>();

                    // Create new number and collect fields
                    var phoneNumber = new PhoneNumber()
                    {
                        Number = collect.CollectField(fieldName: "Number", previousValue: "", required: false),
                        Type = collect.CollectField(fieldName: "Type", previousValue: "", required: false)
                    };

                    // Insert number
                    sql.InsertPhoneNumbers(contact, phoneNumbers);

                    break;
                default:
                    // Check if input is number and output the phone number ID
                    bool isNumber;
                    isNumber = int.TryParse(input, out int id);

                    // Edit existing number if user typed in phone number ID
                    if (isNumber)
                    {
                        var phoneNumberNumber = collect.CollectField(fieldName: "Number", previousValue: contact.PhoneNumbers.Find(number => number.ID == id).Number, required: false);
                        var phoneNumberType = collect.CollectField(fieldName: "Type", previousValue: contact.PhoneNumbers.Find(type => type.ID == id).Type, required: false);
                        sql.UpdatePhoneNumber(contact.ID, phoneNumberNumber, phoneNumberType);
                    }

                    break;
            }
        }
    }
}
