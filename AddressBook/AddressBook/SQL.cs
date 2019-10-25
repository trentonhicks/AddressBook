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

        /// <summary>
        /// Gets all the contacts from the database
        /// </summary>
        /// <returns></returns>
        public List<Contact> GetContacts()
        {
            // Store db contacts to list
            var contacts = new List<Contact>();

            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"SELECT ID, FirstName, LastName FROM Contacts";
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

            // Get Phone Numbers for all the contacts
            contacts = GetPhoneNumbers(contacts);

            // Get Emails for all the contacts
            contacts = GetEmails(contacts);

            // Get Addresses for all the contacts
            contacts = GetAddresses(contacts);

            return contacts;
        }

        /// <summary>
        /// Get all phone numbers for the contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public List<Contact> GetPhoneNumbers(List<Contact> contacts)
        {
            foreach(var contact in contacts)
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $@"SELECT ID, Number, [Type] FROM PhoneNumbers WHERE ContactID = ${contact.ID}";
                    command.CommandType = CommandType.Text;

                    using (var reader = command.ExecuteReader())
                    {
                        // Loop through all contacts in the database
                        while (reader.Read())
                        {
                            var phoneNumber = new PhoneNumber()
                            {
                                ID = (int)reader["ID"],
                                Number = reader["Number"] != DBNull.Value ? (string)reader["Number"] : "",
                                Type = reader["Type"] != DBNull.Value ? (string)reader["Type"] : ""
                            };

                            // Add phone numbers to contact's list
                            contact.PhoneNumbers.Add(phoneNumber);
                        }
                    }
                }

                _connection.Close();
            }

            // Return contacts after phone numbers are added
            return contacts;
        }

        /// <summary>
        /// Get all emails for the contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public List<Contact> GetEmails(List<Contact> contacts)
        {
            foreach (var contact in contacts)
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $@"SELECT ID, [Text], [Type] FROM Emails WHERE ContactID = ${contact.ID}";
                    command.CommandType = CommandType.Text;

                    using (var reader = command.ExecuteReader())
                    {
                        // Loop through all contacts in the database
                        while (reader.Read())
                        {
                            var email = new Email()
                            {
                                ID = (int)reader["ID"],
                                Text = reader["Text"] != DBNull.Value ? (string)reader["Text"] : "",
                                Type = reader["Type"] != DBNull.Value ? (string)reader["Type"] : ""
                            };

                            // Add emails to contact's list
                            contact.Emails.Add(email);
                        }
                    }
                }

                _connection.Close();
            }

            // Return contacts after emails are added
            return contacts;
        }

        /// <summary>
        /// Get all addresses for the contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public List<Contact> GetAddresses(List<Contact> contacts)
        {
            foreach (var contact in contacts)
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $@"SELECT ID, StreetName, City, [State], ZipCode, [Type] FROM Addresses WHERE ContactID = ${contact.ID}";
                    command.CommandType = CommandType.Text;

                    using (var reader = command.ExecuteReader())
                    {
                        // Loop through all contacts in the database
                        while (reader.Read())
                        {
                            var address = new Address()
                            {
                                ID = (int)reader["ID"],
                                StreetName = reader["StreetName"] != DBNull.Value ? (string)reader["StreetName"] : "",
                                City = reader["City"] != DBNull.Value ? (string)reader["City"] : "",
                                State = reader["State"] != DBNull.Value ? (string)reader["State"] : "",
                                ZipCode = reader["ZipCode"] != DBNull.Value ? (string)reader["ZipCode"] : "",
                                Type = reader["Type"] != DBNull.Value ? (string)reader["Type"] : ""
                            };

                            // Add addresses to the contact's list
                            contact.Addresses.Add(address);
                        }
                    }
                }

                _connection.Close();
            }

            // Return contacts after emails are added
            return contacts;
        }

        /// <summary>
        /// Displays a list of all contacts from the database.
        /// </summary>
        public void DisplayContactsList()
        {
            var contacts = GetContacts();

            foreach(var contact in contacts)
            {
                Console.WriteLine($"{contact.ID} {contact.FirstName} {contact.LastName}");
            }
        }

        /// <summary>
        /// Gets a contact from the database or returns null if contact doesn't exist.
        /// </summary>
        /// <param name="ID"></param>
        public Contact GetContact(int ID)
        {
            var contacts = GetContacts();

            // Check for contact in the database by comparing with ID
            foreach(var contact in contacts)
            {
                if(contact.ID == ID)
                {
                    return contact;
                }
            }
            return null;
        }

        public void CreateContact(Contact contact)
        {
            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"INSERT INTO Contacts(FirstName, LastName) OUTPUT Inserted.ID VALUES (@firstName, @lastName)";

                // Insert values
                command.Parameters.AddWithValue("@firstName", contact.FirstName);
                command.Parameters.AddWithValue("@lastName", contact.LastName == "NULL" ? (object)DBNull.Value : contact.LastName);

                contact.ID = Convert.ToInt32(command.ExecuteScalar().ToString());
            }

            _connection.Close();

            // Addresses
            foreach(var address in contact.Addresses)
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $@"INSERT INTO Addresses(ContactID, StreetName, City, State, ZipCode, [Type])
                                           VALUES (@contactID, @streetName, @city, @state, @zipCode, @type)";
                    command.CommandType = CommandType.Text;

                    // Insert values
                    command.Parameters.AddWithValue("@contactID", contact.ID);
                    command.Parameters.AddWithValue("@streetName", address.StreetName == "NULL" ? (object)DBNull.Value : address.StreetName);
                    command.Parameters.AddWithValue("@city", address.City == "NULL" ? (object)DBNull.Value : address.City);
                    command.Parameters.AddWithValue("@state", address.State == "NULL" ? (object)DBNull.Value : address.State);
                    command.Parameters.AddWithValue("@zipCode", address.ZipCode == "NULL" ? (object)DBNull.Value : address.ZipCode);
                    command.Parameters.AddWithValue("@type", address.Type == "NULL" ? (object)DBNull.Value : address.Type);

                    command.ExecuteNonQuery();
                }

                _connection.Close();
            }

            // Phone Numbers
            foreach (var phoneNumber in contact.PhoneNumbers)
                {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $@"INSERT INTO PhoneNumbers(ContactID, Number, [Type]) VALUES (@contactID, @number, @type)";
                    command.CommandType = CommandType.Text;

                    // Insert values
                    command.Parameters.AddWithValue("@contactID", contact.ID);
                    command.Parameters.AddWithValue("@number", phoneNumber.Number == "NULL" ? (object)DBNull.Value : phoneNumber.Number);
                    command.Parameters.AddWithValue("@type", phoneNumber.Type == "NULL" ? (object)DBNull.Value : phoneNumber.Type);

                    command.ExecuteNonQuery();
                }

                _connection.Close();
            }

            // Emails
            foreach (var email in contact.Emails)
            {
                _connection.Open();

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $@"INSERT INTO Emails(ContactID, Text, [Type]) VALUES (@contactID, @text, @type)";
                    command.CommandType = CommandType.Text;

                    // Insert values
                    command.Parameters.AddWithValue("@contactID", contact.ID);
                    command.Parameters.AddWithValue("@text", email.Text == "NULL" ? (object)DBNull.Value : email.Text);
                    command.Parameters.AddWithValue("@type", email.Type == "NULL" ? (object)DBNull.Value : email.Type);

                    command.ExecuteNonQuery();
                }

                _connection.Close();
            }
        } // End of CreateContact

    }
}
