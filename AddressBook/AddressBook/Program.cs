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
            
        }



        //Menu screen when 1.Create Contact has been selected
        public static void CreateContact(Collect collect, SQL sql)
        {
            while (true)
            {


                //creating variables for adding user input to the classes.//
                var contact = new Contact();

                while (true)
                {
                    Console.Clear();
                    //display//
                    Console.WriteLine("0.=>Back to Menu");

                    Console.WriteLine($"1.FirstName:{contact.FirstName}");

                    Console.WriteLine($"2.Last Name:{contact.LastName}");

                    Console.WriteLine($"3.Phone Number:");
                    foreach (var phoneNumbers in contact.PhoneNumbers)
                    {
                        Console.WriteLine("" +
                            $"\nNumber:{phoneNumbers.Number}" +
                            $"\nType:{phoneNumbers.Type}");
                    }

                    Console.WriteLine($"4.Email:");

                    Console.WriteLine($"5.Address:\n");


                    foreach (var addresses in contact.Addresses)
                    {
                        Console.WriteLine("" +
                            $"\nStreet Name:{addresses.StreetName}" +
                            $"\nCity:{addresses.City}" +
                            $"\nState:{addresses.State}" +
                            $"\nStreet Name:{addresses.ZipCode}");
                    }
                    Console.WriteLine("6.Save\n");
                //for (var i = 0; i < contact.Addresses.Count; i++)
                //{
                //    Console.Write($"\n\nStreetName: {contact.Addresses[i].StreetName + "\n"}City:{contact.Addresses[i].City + "\n"}State:{contact.Addresses[i].State + "\n"}Zip:{ contact.Addresses[i].Zip + " "}");
                //}

                Console.Write("Number selected: ");

                    var input = Console.ReadLine();

                    //adding input to database once case is declared.//
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
                            //PhoneNumberScreen();
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
                            //AddressScreen();
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

        /* public static void PhoneNumberScreen()
         {
             var collect = new Collect();
             var phoneNumber = new PhoneNumber();

             phoneNumber.Number = collect.CollectField(fieldName: "Number", previousValue: phoneNumber.Number, required: false);
             phoneNumber.Type = collect.CollectField(fieldName: "Type", previousValue: phoneNumber.Type, required: false);
         }
         public static void AddressScreen()
         {
             var collect = new Collect();
             var address = new Address();

             address.StreetName = collect.CollectField(fieldName: "Street Name", previousValue: address.StreetName, required: false);
             address.City = collect.CollectField(fieldName: "City", previousValue: address.City, required: false);
             address.State = collect.CollectField(fieldName: "State", previousValue: address.State, required: false);
             address.ZipCode = collect.CollectField(fieldName: "ZipCode", previousValue: address.ZipCode, required: false);
             address.Type = collect.CollectField(fieldName: "Type", previousValue: address.Type, required: false);
         }*/

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
                Console.Write("" +
                    "0.=>Return to previous menu" +
                    "\n\nEnter the ID of the contact you wish to View/Edit:");
                sql.DisplayContactsList();

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
            var db = new SQL();
            var contacts = db.GetContacts();
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

            //Console.WriteLine($"{contacts.ID} {contact.FirstName} {contact.LastName}");

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
