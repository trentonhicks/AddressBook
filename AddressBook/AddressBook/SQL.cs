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
        private static SqlConnection _connection = new SqlConnection(@"Server=tcp:codeflip.database.windows.net,1433;Initial Catalog=squeakyninja;Persist Security Info=False;User ID=steve;Password=Redcoin1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

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
                            FirstName = reader["FirstName"] == DBNull.Value ? "" : (string)reader["FirstName"],
                            LastName = reader["LastName"] == DBNull.Value ? "" : (string)reader["LastName"]
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
            foreach (var contact in contacts)
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

            foreach (var contact in contacts)
            {
                Console.WriteLine($"{contact.ID} {contact.FirstName} {contact.LastName}");
            }
        }

        public void DisplayPhoneNumbers(Contact contact)
        {
            foreach (var phoneNumber in contact.PhoneNumbers)
            {
                Console.WriteLine($"{phoneNumber.ID}:{phoneNumber.Number} / {phoneNumber.Type}");
            }
        }

        public void DisplayEmails(Contact contact)
        {
            foreach (var emails in contact.Emails)
            {
                Console.WriteLine($"{emails.ID}:{emails.Text} / {emails.Type}");
            }
        }

        public void DisplayAddresses(Contact contact)
        {
            foreach (var address in contact.Addresses)
            {
                Console.WriteLine($"{address.ID}: {address.StreetName}, {address.City}, {address.State} {address.ZipCode}\nType: {address.Type}\n");
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
            foreach (var contact in contacts)
            {
                if (contact.ID == ID)
                {
                    return contact;
                }
            }
            return null;
        }

        /// <summary>
        /// Creates a contact in SQL
        /// </summary>
        /// <param name="contact"></param>
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
            InsertAddresses(contact, contact.Addresses);

            // Phone Numbers
            InsertPhoneNumbers(contact, contact.PhoneNumbers);

            // Emails
            InsertEmails(contact, contact.Emails);
        }

        /// <summary>
        /// Insert new phone numbers into an existing contact
        /// </summary>
        /// <param name="contact"></param>
        public void InsertAddresses(Contact contact, List<Address> addresses)
        {
            foreach (var address in addresses)
            {
                if(address.StreetName != "NULL" || address.City != "NULL" || address.State != "NULL" || address.ZipCode != "NULL" || address.Type != "NULL")
                {
                    _connection.Open();

                    using (var command = _connection.CreateCommand())
                    {
                        command.CommandText = $@"INSERT INTO Addresses(ContactID, StreetName, City, [State], ZipCode, [Type])
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
            }
        }

        /// <summary>
        /// Insert new phone numbers into an existing contact
        /// </summary>
        /// <param name="ID"></param>
        public void InsertPhoneNumbers(Contact contact, List<PhoneNumber> phoneNumbers)
        {
            // Phone Numbers
            foreach (var phoneNumber in phoneNumbers)
            {
                if(phoneNumber.Number != "NULL" || phoneNumber.Type != "NULL")
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
            }
        }

        public void InsertEmails(Contact contact, List<Email> emails)
        {
            foreach (var email in emails)
            {
                if(email.Text != "NULL" || email.Type != "NULL")
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
            }
        }

        // Update first name
        public void UpdateFirstName(int ID, string firstname)
        {

            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Contacts SET FirstName = '{firstname}' WHERE ID = {ID}";

                command.ExecuteNonQuery();

            }
            _connection.Close();
        }

        //update last name//
        public void UpdateLastName(int ID, string lastname)
        {

            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Contacts SET LastName = '{lastname}' WHERE ID = {ID}";

                command.ExecuteNonQuery();

            }
            _connection.Close();

        }


        public void UpdatePhoneNumber(int FKID, int phoneNumberID, string phoneNumber, string type)
        {
            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE PhoneNumbers SET Number = '{phoneNumber}', [Type] = '{type}' WHERE ContactID = {FKID} AND ID = {phoneNumberID}";
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public void UpdateEmail(int FKID, int emailID, string email, string type)
        {
            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Emails SET Text = '{email}', [Type] = '{type}' WHERE ContactID = {FKID} AND ID = {emailID}";
                command.ExecuteNonQuery();
            }
            _connection.Close();

        }

        public void UpdateStreetName (int FKID, int streetNameID, string streetName)
        {
            int ID;

            _connection.Open();

            using(var command = _connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Addresses SET StreetName = '{streetName}' WHERE ContactID = {FKID} AND ID = {streetNameID}";
                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        public void UpdateState(int FKID, int stateID, string state)
        {

            int ID;

            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Addresses SET StreetName = '{state}' WHERE ContactID = {FKID} AND ID = {stateID}";
                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        public void UpdateCity(int FKID, int cityID, string city)
        {
            int ID;

            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Addresses SET City = '{city}' WHERE ContactID = {FKID} AND ID = {cityID}";
                command.ExecuteNonQuery();
            }

            _connection.Close();
        }
       
        public void UpdateZipCode(int FKID, string zipCode, int zipCodeID)
        {
            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Addresses SET ZipCode= '{zipCode}' WHERE ContactID = {FKID} AND ID = {zipCodeID}";
                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        public void UpdateAddressType(int FKID, string addressType, int addressTypeID)
        {
            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Addresses SET Type= '{addressType}' WHERE ContactID = {FKID} AND ID = {addressTypeID}";
                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        /// <summary>
        /// Deletes a phone number from the database
        /// </summary>
        /// <param name="FKID"></param>
        /// <param name="phoneNumberID"></param>
        public void DeletePhoneNumber(int FKID, int phoneNumberID)
        {
            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"DELETE FROM PhoneNumbers WHERE ContactID = {FKID} AND ID = {phoneNumberID}";
                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        /// <summary>
        /// Deletes an email from the database
        /// </summary>
        /// <param name="FKID"></param>
        /// <param name="emailID"></param>
        public void DeleteEmail(int FKID, int emailID)
        {
            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"DELETE FROM Emails WHERE ContactID = {FKID} AND ID = {emailID}";
                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        public void DeleteAddress(int FKID, int addressID)
        {
            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"DELETE FROM Addresses WHERE ContactID = {FKID} AND ID = {addressID}";
                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        public void DeleteContactUser(int ID)
        {
            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"DELETE FROM Contacts WHERE ID = {ID}";
                command.ExecuteNonQuery();
            }

            _connection.Close();
        }

        public void DeleteFullContact(int ID)
        {
            _connection.Open();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $@"DELETE FROM Addresses WHERE ContactID = {ID}";
                command.ExecuteNonQuery();

                command.CommandText = $@"DELETE FROM PhoneNumbers WHERE ContactID = {ID}";
                command.ExecuteNonQuery();

                command.CommandText = $@"DELETE FROM Emails WHERE ContactID = {ID}";
                command.ExecuteNonQuery();

            }

            _connection.Close();

            DeleteContactUser(ID);
        }
    }
}