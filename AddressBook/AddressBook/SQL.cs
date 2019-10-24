using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AddressBook
{
    class SQL
    {
        // SQL connection string
        private static SqlConnection _connection = new SqlConnection(@"Data Source=.; Initial Catalog=AddressBook2.0; Integrated Security=SSPI;");

        // Get Contacts
        public List<Contact> GetContacts()
        {
            // Store db contacts to list
            var contacts = new List<Contact>();

            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"SELECT ID, FirstName, LastName, Contacts";
                command.CommandType = CommandType.Text;

                using (var reader = command.ExecuteReader())
                {
                    // Loop through all contacts in the database
                    while (reader.Read())
                    {
                        // Create contact
                        var contact = new Contact()
                        {
                            ID = (int)reader["ID"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"]
                        };
                        // Add contact to list
                        contacts.Add(contact);
                    }
                }
            }
            _connection.Close();
            return contacts;
        }
        /// <summary>
        /// Display list of contacts from the database
        /// </summary>
        public void DisplayContactsList()
        {
            var contacts = GetContacts();

            foreach(var contact in contacts)
            {
                Console.WriteLine($"{contact.ID} {contact.FirstName} {contact.LastName}");
            }
        }

    }
}
