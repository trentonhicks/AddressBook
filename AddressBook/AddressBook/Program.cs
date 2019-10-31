using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace AddressBook
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var collect = new Collect();
            var sql = new SQL(configuration.GetConnectionString("Storage"));

            Menu(collect, sql);
        }

        //This is the first menu that is presented to you
        public static void Menu(Collect collect, SQL sql)
        {
            Console.Clear();

            while (true)
            {
                Console.WriteLine("1.Create Contact: ");

                Console.WriteLine("2.Contact List: ");

                Console.Write("\nSelect an option:");


                var input = Console.ReadLine();

                Console.Clear();

                switch (input)
                {
                    case "1":
                        CreateContact(collect, sql);
                        break;

                    case "2":
                        ContactListSelection(sql, collect);
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
                Console.WriteLine($"1.FirstName: {(contact.FirstName == "No name" ? "" : contact.FirstName)}");
                Console.WriteLine($"2.Last Name: {(contact.LastName == "NULL" ? "" : contact.LastName)}");
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
                        $"Street Name:{addresses.StreetName}" +
                        $" / City:{addresses.City}" +
                        $" / State:{addresses.State}" +
                        $" / ZipCode:{addresses.ZipCode}\n");
                }
                Console.WriteLine("\n6.Save");

                Console.Write("\nSelect an option:");

                var input = Console.ReadLine();

                //adding input to database once case is declared.
                switch (input)
                {
                    case "0":
                        Console.Clear();
                        Main(new string[1]);
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
                        CreateContact(collect, sql);
                        break;
                }
            }
        }
        public static void ContactListSelection(SQL sql, Collect collect)
        {
            while (true)
            {
                Console.Clear();
                DisplayContactsList(sql);

                Console.WriteLine("" +
                    "\n0.=>Back to Menu\n" +
                    "1.View/Edit\n" +
                    "2.Delete:\n");

                Console.Write("\nSelect an option:");

                var input = Console.ReadLine();


                Console.Clear();

                switch (input)
                {
                    case "0":
                        return;
                    case "1":
                        EditContact(collect, sql);
                        break;
                    case "2":
                        DeleteContact(collect, sql);
                        break;
                }
            }
        }
        public static void DeleteContact(Collect collect, SQL sql)
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("" + "0.=>Previous menu");

                DisplayContactsList(sql);

                var ID = collect.CollectID("\nEnter the ID of the contact you want to delete");

                if (ID != 0)
                {
                    var contact = sql.GetContact(ID);

                    // If contact exists, delete it
                    if (contact != null)
                    {
                        Console.Clear();
                        Console.WriteLine("Are you sure you want to delete?\n Press 1 for Yes: \n Press 2 to cancel: \n");

                        Console.Write("Select: ");

                        var userInput = Console.ReadLine();

                        switch (userInput)
                        {
                            case "1":
                                sql.DeleteFullContact(ID);
                                return;
                            case "2":

                                break;
                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }

        public static void EditContact(Collect collect, SQL sql)
        {
            DisplayContactsList(sql);

            Console.WriteLine("0. =>Previous menu");

            var ID = collect.CollectID("\nEnter ID of contact you'd like to edit"); //no option to go back

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

                    Console.Write("\nSelect an option: ");
                    var input = Console.ReadLine();

                    //adding input to database once case is declared.
                    switch (input)
                    {
                        case "0":
                            return;
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
                            contact = sql.GetContact(contact.ID);
                            break;
                        case "4":
                            
                            EditEmails(collect, sql, contact);
                            contact = sql.GetContact(contact.ID);
                            
                            break;
                        case "5":

                            EditAddresses(collect, sql, contact);
                            contact = sql.GetContact(contact.ID);

                            break;

                    }
                }
            }

            // Contact doesn't exist
            else
            {
                Console.WriteLine("Contact doesn't exist");
            }
        }
        public static void EditPhoneNumbers(Collect collect, SQL sql, Contact contact)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("0. => Go back");
                Console.WriteLine("add. add a new phone number");
                Console.WriteLine("delete. delete a phone number");


                DisplayPhoneNumbers(contact);

                Console.WriteLine("Enter the ID of the number you wish you wish to edit");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        Console.Clear();
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

                        // Add number to list
                        phoneNumbers.Add(phoneNumber);

                        // Insert number
                        sql.InsertPhoneNumbers(contact, phoneNumbers);
                        contact = sql.GetContact(contact.ID);

                        break;

                    case "delete":
                        Console.Clear();
                        DisplayPhoneNumbers(contact);
                        var phoneNumberID = collect.CollectID("Enter the ID of the phone number you want to delete");
                        sql.DeletePhoneNumber(contact.ID, phoneNumberID);
                        contact = sql.GetContact(contact.ID);
                        break;

                    default:
                        // Check if input is number and output the phone number ID
                        bool isNumber;
                        isNumber = int.TryParse(input, out int numberID);

                        // Edit existing number if user typed in phone number ID
                        if (isNumber)
                        {
                            var phoneNumberNumber = collect.CollectField(fieldName: "Number", previousValue: contact.PhoneNumbers.Find(number => number.ID == numberID).Number, required: false);
                            var phoneNumberType = collect.CollectField(fieldName: "Type", previousValue: contact.PhoneNumbers.Find(type => type.ID == numberID).Type, required: false);
                            sql.UpdatePhoneNumber(contact.ID, numberID, phoneNumberNumber, phoneNumberType);
                            contact = sql.GetContact(contact.ID);
                        }

                        break;
                }
            }
        }
        public static void EditEmails(Collect collect, SQL sql, Contact contact)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("0. => Go back");
                Console.WriteLine("add. add a new email");
                Console.WriteLine("delete. delete an email");


                DisplayEmails(contact);

                Console.Write("Enter the ID of the email you wish you wish to edit:");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        Console.Clear();
                        return;
                    case "add":
                        // Store new numbers in a list
                        var emails = new List<Email>();

                        // Create new number and collect fields
                        var email = new Email()
                        {
                            Text = collect.CollectField(fieldName: "Text", previousValue: "", required: false),
                            Type = collect.CollectField(fieldName: "Type", previousValue: "", required: false)
                        };

                        emails.Add(email);

                        // Insert number
                        sql.InsertEmails(contact, emails); // check insertemails
                        contact = sql.GetContact(contact.ID);
                        break;

                    case "delete":
                        Console.Clear();
                        DisplayEmails(contact);
                        var deleteEmailID = collect.CollectID("Enter the ID of the email you want to delete");
                        sql.DeleteEmail(contact.ID, deleteEmailID);
                        contact = sql.GetContact(contact.ID);
                        break;

                    default:
                        // Check if input is number and output the email ID
                        bool isNumber;
                        isNumber = int.TryParse(input, out int emailID);

                        // Edit existing email if user typed in email ID
                        if (isNumber)
                        {
                            var emailText = collect.CollectField(fieldName: "text", previousValue: contact.Emails.Find(text => text.ID == emailID).Text, required: false);
                            var emailType = collect.CollectField(fieldName: "Type", previousValue: contact.Emails.Find(type => type.ID == emailID).Type, required: false);
                            sql.UpdateEmail(contact.ID, emailID, emailText, emailType);
                            contact = sql.GetContact(contact.ID);
                        }

                        break;
                }
            }
        }
        public static void EditAddresses(Collect collect, SQL sql, Contact contact)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("0. => Go back");
                Console.WriteLine("add. add a new address");
                Console.WriteLine("delete. delete an address\n");


                DisplayAddresses(contact);

                Console.Write("\nEnter the ID of the address you wish you wish to edit:");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        Console.Clear();
                        return;

                    case "add":
                        // Store new numbers in a list
                        var addresses = new List<Address>();

                        // Create new number and collect fields
                        var address = new Address()
                        {
                            StreetName = collect.CollectField(fieldName: "Street Name", previousValue: "", required: false),
                            City = collect.CollectField(fieldName: "City", previousValue: "", required: false),
                            State = collect.CollectField(fieldName: "State", previousValue: "", required: false),
                            ZipCode = collect.CollectField(fieldName: "ZipCode", previousValue: "", required: false),
                            Type = collect.CollectField(fieldName: "Type", previousValue: "", required: false),
                        };

                        addresses.Add(address);

                        // Insert number
                        sql.InsertAddresses(contact, addresses);
                        contact = sql.GetContact(contact.ID);
                        break;

                    case "delete":
                        DisplayAddresses(contact);
                        var id = collect.CollectID("Enter the ID of the address you want to delete");
                        sql.DeleteAddress(contact.ID, id);
                        contact = sql.GetContact(contact.ID);
                        break;

                    default:
                        // Check if input is number and output the address ID
                        bool isNumber;
                        isNumber = int.TryParse(input, out int addressID);

                        // Edit existing address if user typed in address ID
                        if (isNumber)
                        {
                            var addressStreetName = collect.CollectField(fieldName: "StreetName", previousValue: contact.Addresses.Find(streetName => streetName.ID == addressID).StreetName, required: false);
                            var addressCity = collect.CollectField(fieldName: "City", previousValue: contact.Addresses.Find(city => city.ID == addressID).City, required: false);
                            var addressState = collect.CollectField(fieldName: "State", previousValue: contact.Addresses.Find(state => state.ID == addressID).State, required: false);
                            var addressZipCode = collect.CollectField(fieldName: "ZipCode", previousValue: contact.Addresses.Find(zipCode => zipCode.ID == addressID).ZipCode, required: false);
                            var addressType = collect.CollectField(fieldName: "Type", previousValue: contact.Addresses.Find(type => type.ID == addressID).Type, required: false);

                            sql.UpdateStreetName(contact.ID, addressID, addressStreetName);
                            sql.UpdateCity(contact.ID, addressID, addressCity);
                            sql.UpdateState(contact.ID, addressID, addressState);
                            sql.UpdateZipCode(contact.ID, addressZipCode, addressID);
                            sql.UpdateAddressType(contact.ID, addressType, addressID);
                            contact = sql.GetContact(contact.ID);
                        }

                        break;
                }
            }
        }

        public Address CollectAddress(Contact contact, Collect collect)
        {
            while (true)
            {
                var address = new Address();

                Console.WriteLine("1.Street Name: ");

                Console.WriteLine("2.City: ");

                Console.WriteLine("3.State: ");

                Console.WriteLine("4.Zip Code: ");

                Console.WriteLine("5.Type: ");

                Console.Write("\nSelect an option: ");

                var inputAddress = Console.ReadLine();

                if (inputAddress == "1")
                {
                    address.StreetName = collect.CollectField(fieldName: "Street Name", previousValue: address.StreetName, required: false);
                }
                else if (inputAddress == "2")
                {
                    address.City = collect.CollectField(fieldName: "City", previousValue: address.City, required: false);
                }
                else if (inputAddress == "3")
                {
                    address.State = collect.CollectField(fieldName: "State", previousValue: address.State, required: false);
                }
                else if (inputAddress == "4")
                {
                    address.ZipCode = collect.CollectField(fieldName: "ZipCode", previousValue: address.ZipCode, required: false);
                }
                else if (inputAddress == "5")
                {
                    address.Type = collect.CollectField(fieldName: "Type", previousValue: address.Type, required: false);
                }

                if (inputAddress == "0")
                {
                    return address;
                }
            }
        }

        /// <summary>
        /// Displays a list of all contacts from the database.
        /// </summary>
        public static void DisplayContactsList(SQL sql)
        {
            var contacts = sql.GetContacts();

            foreach (var contact in contacts)
            {
                Console.WriteLine($"{contact.ID} {contact.FirstName} {contact.LastName}");
            }
        }

        public static void DisplayPhoneNumbers(Contact contact)
        {
            foreach (var phoneNumber in contact.PhoneNumbers)
            {
                Console.WriteLine($"{phoneNumber.ID}:{phoneNumber.Number} / {phoneNumber.Type}");
            }
        }

        public static void DisplayEmails(Contact contact)
        {
            foreach (var emails in contact.Emails)
            {
                Console.WriteLine($"{emails.ID}:{emails.Text} / {emails.Type}");
            }
        }

        public static void DisplayAddresses(Contact contact)
        {
            foreach (var address in contact.Addresses)
            {
                Console.WriteLine($"{address.ID}: {address.StreetName}, {address.City}, {address.State} {address.ZipCode}\nType: {address.Type}\n");
            }
        }

    }
}
