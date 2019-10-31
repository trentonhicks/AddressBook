using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AddressBook
{
    class SQL
    {
        public SQL(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }

        // SQL connection string
        public static SqlConnection Connection { get; set; }

        /// <summary>
        /// Gets all the contacts from the database
        /// </summary>
        /// <returns></returns>
        public List<Contact> GetContacts()
        {
            // Store db contacts to list
            var contacts = new List<Contact>();

            Connection.Open();

            using (var command = Connection.CreateCommand())
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
            Connection.Close();

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
                Connection.Open();

                using (var command = Connection.CreateCommand())
                {
                    command.CommandText = $@"SELECT ID, Number, [Type] FROM PhoneNumbers WHERE ContactID = @contactID";
                    command.Parameters.AddWithValue("@contactID", contact.ID);
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

                            // Add phone numbers to contacts list
                            contact.PhoneNumbers.Add(phoneNumber);
                        }
                    }
                }

                Connection.Close();
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
                Connection.Open();

                using (var command = Connection.CreateCommand())
                {
                    command.CommandText = $@"SELECT ID, [Text], [Type] FROM Emails WHERE ContactID = @contactID";
                    command.Parameters.AddWithValue("@contactID", contact.ID);
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

                            // Add emails to contacts list
                            contact.Emails.Add(email);
                        }
                    }
                }

                Connection.Close();
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
                Connection.Open();

                using (var command = Connection.CreateCommand())
                {
                    command.CommandText = $@"SELECT ID, StreetName, City, [State], ZipCode, [Type] FROM Addresses WHERE ContactID = @contactID";
                    command.Parameters.AddWithValue("@contactID", contact.ID);
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

                            // Add addresses to the contacts list
                            contact.Addresses.Add(address);
                        }
                    }
                }

                Connection.Close();
            }

            // Return contacts after emails are added
            return contacts;
        }

        /// <summary>
        /// Gets a contact from the database or returns null if contact doesnt exist.
        /// </summary>
        /// <param name="ID"></param>
        public Contact GetContact(int ID)
        {
            var contact = new Contact();

            Connection.Open();
            
            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"SELECT ID, FirstName, LastName FROM Contacts WHERE ID = @contactID";

                command.Parameters.AddWithValue("@contactID", ID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contact.ID = (int)reader["ID"];
                        contact.FirstName = reader["FirstName"] == DBNull.Value ? "" : (string)reader["FirstName"];
                        contact.LastName = reader["LastName"] == DBNull.Value ? "" : (string)reader["LastName"];
                    }
                }

                //phoneNumbers
                //emails
                //addresses
            }
            Connection.Close();

            return contact;
        }

        /// <summary>
        /// Creates a contact in SQL
        /// </summary>
        /// <param name="contact"></param>
        public void CreateContact(Contact contact)
        {
            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"INSERT INTO Contacts(FirstName, LastName) OUTPUT Inserted.ID VALUES (@firstName, @lastName)";

                // Insert values
                command.Parameters.AddWithValue("@firstName", contact.FirstName);
                command.Parameters.AddWithValue("@lastName", contact.LastName == "NULL" ? (object)DBNull.Value : contact.LastName);

                contact.ID = Convert.ToInt32(command.ExecuteScalar().ToString());
            }

            Connection.Close();

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
                    Connection.Open();

                    using (var command = Connection.CreateCommand())
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

                    Connection.Close();
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
                    Connection.Open();

                    using (var command = Connection.CreateCommand())
                    {
                        command.CommandText = $@"INSERT INTO PhoneNumbers(ContactID, Number, [Type]) VALUES (@contactID, @number, @type)";
                        command.CommandType = CommandType.Text;

                        // Insert values
                        command.Parameters.AddWithValue("@contactID", contact.ID);
                        command.Parameters.AddWithValue("@number", phoneNumber.Number == "NULL" ? (object)DBNull.Value : phoneNumber.Number);
                        command.Parameters.AddWithValue("@type", phoneNumber.Type == "NULL" ? (object)DBNull.Value : phoneNumber.Type);

                        command.ExecuteNonQuery();
                    }

                    Connection.Close();
                }
            }
        }

        public void InsertEmails(Contact contact, List<Email> emails)
        {
            foreach (var email in emails)
            {
                if(email.Text != "NULL" || email.Type != "NULL")
                {
                    Connection.Open();

                    using (var command = Connection.CreateCommand())
                    {
                        command.CommandText = $@"INSERT INTO Emails(ContactID, Text, [Type]) VALUES (@contactID, @text, @type)";
                        command.CommandType = CommandType.Text;

                        // Insert values
                        command.Parameters.AddWithValue("@contactID", contact.ID);
                        command.Parameters.AddWithValue("@text", email.Text == "NULL" ? (object)DBNull.Value : email.Text);
                        command.Parameters.AddWithValue("@type", email.Type == "NULL" ? (object)DBNull.Value : email.Type);

                        command.ExecuteNonQuery();
                    }

                    Connection.Close();
                }
            }
        }

        // Update first name
        public void UpdateFirstName(int ID, string firstname)
        {

            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = @"UPDATE Contacts SET FirstName = @firstname WHERE ID = @ID";
                command.Parameters.AddWithValue("@firstname", firstname);
                command.Parameters.AddWithValue("@ID", ID);
                command.ExecuteNonQuery();

            }
            Connection.Close();
        }

        //update last name//
        public void UpdateLastName(int ID, string lastname)
        {

            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Contacts SET LastName = @lastname WHERE ID = @ID";
                command.Parameters.AddWithValue("@lastname", lastname);
                command.Parameters.AddWithValue("@ID", ID);
                command.ExecuteNonQuery();

            }
            Connection.Close();

        }


        public void UpdatePhoneNumber(int FKID, int phoneNumberID, string phoneNumber, string type)
        {
            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE PhoneNumbers SET Number = @phoneNumber, [Type] = @type WHERE ContactID = @FKID AND ID = @phoneNumberID";
                command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@FKID", FKID);
                command.Parameters.AddWithValue("@phoneNumberID", phoneNumberID);
                command.ExecuteNonQuery();
            }
            Connection.Close();
        }

        public void UpdateEmail(int FKID, int emailID, string email, string type)
        {
            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Emails SET Text = @email, [Type] = @type WHERE ContactID = @FKID AND ID = @emailID";
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@FKID", FKID);
                command.Parameters.AddWithValue("@emailID", emailID);
                command.ExecuteNonQuery();
            }
            Connection.Close();

        }

        public void UpdateStreetName (int FKID, int streetNameID, string streetName)
        {
            Connection.Open();

            using(var command = Connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Addresses SET StreetName = @streetName WHERE ContactID = @FKID0 AND ID = @streetNameID";
                command.Parameters.AddWithValue("@streetName", streetName);
                command.Parameters.AddWithValue("@FKID", FKID);
                command.Parameters.AddWithValue("@streetNameID", streetNameID);
                command.ExecuteNonQuery();
            }

            Connection.Close();
        }

        public void UpdateState(int FKID, int stateID, string state)
        {
            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Addresses SET StreetName = @state WHERE ContactID = @FKID AND ID = @stateID";
                command.Parameters.AddWithValue("@state", state);
                command.Parameters.AddWithValue("@FKID", FKID);
                command.Parameters.AddWithValue("@stateID", stateID);
                command.ExecuteNonQuery();
            }

            Connection.Close();
        }

        public void UpdateCity(int FKID, int cityID, string city)
        {
            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Addresses SET City = @city WHERE ContactID = @FKID AND ID = @cityID";
                command.Parameters.AddWithValue("@city", city);
                command.Parameters.AddWithValue("@FKID", FKID);
                command.Parameters.AddWithValue("@cityID", cityID);
                command.ExecuteNonQuery();
            }

            Connection.Close();
        }
       
        public void UpdateZipCode(int FKID, string zipCode, int zipCodeID)
        {
            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Addresses SET ZipCode= @zipCode WHERE ContactID = @FKID AND ID = @zipCodeID";
                command.Parameters.AddWithValue("@zipCode", zipCode);
                command.Parameters.AddWithValue("@FKID", FKID);
                command.Parameters.AddWithValue("@zipCodeID", zipCodeID);
                command.ExecuteNonQuery();
            }

            Connection.Close();
        }

        public void UpdateAddressType(int FKID, string addressType, int addressTypeID)
        {
            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"UPDATE Addresses SET Type= @addressType WHERE ContactID = @FKID AND ID = @addressTypeID";
                command.Parameters.AddWithValue("@addressType", addressType);
                command.Parameters.AddWithValue("@FKID", FKID);
                command.Parameters.AddWithValue("@addressTypeID", addressTypeID);
                command.ExecuteNonQuery();
            }

            Connection.Close();
        }

        /// <summary>
        /// Deletes a phone number from the database
        /// </summary>
        /// <param name="FKID"></param>
        /// <param name="phoneNumberID"></param>
        public void DeletePhoneNumber(int FKID, int phoneNumberID)
        {
            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"DELETE FROM PhoneNumbers WHERE ContactID = @FKID AND ID = @phoneNumberID";
                command.Parameters.AddWithValue("@FKID", FKID);
                command.Parameters.AddWithValue("@phoneNumberID", phoneNumberID); ;
                command.ExecuteNonQuery();
            }

            Connection.Close();
        }

        /// <summary>
        /// Deletes an email from the database
        /// </summary>
        /// <param name="FKID"></param>
        /// <param name="emailID"></param>
        public void DeleteEmail(int FKID, int emailID)
        {
            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"DELETE FROM Emails WHERE ContactID = @FKID AND ID = @emailID";
                command.Parameters.AddWithValue("@FKID", FKID);
                command.Parameters.AddWithValue("@emailID", emailID);
                command.ExecuteNonQuery();
            }

            Connection.Close();
        }

        public void DeleteAddress(int FKID, int addressID)
        {
            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"DELETE FROM Addresses WHERE ContactID = @FKID AND ID = @addressID";
                command.Parameters.AddWithValue("@FKID", FKID);
                command.Parameters.AddWithValue("@addressID", addressID);
                command.ExecuteNonQuery();
            }

            Connection.Close();
        }

        public void DeleteContactUser(int ID)
        {
            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"DELETE FROM Contacts WHERE ID = @ID";
                command.Parameters.AddWithValue("@ID", ID);
                command.ExecuteNonQuery();
            }

            Connection.Close();
        }

        public void DeleteFullContact(int ID)
        {
            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = $@"DELETE FROM Addresses WHERE ContactID = @ID";
                command.Parameters.AddWithValue("@ID" , ID);
                command.ExecuteNonQuery();

                command.Parameters.Clear();
                command.CommandText = $@"DELETE FROM PhoneNumbers WHERE ContactID = @ID";
                command.Parameters.AddWithValue("@ID", ID);
                command.ExecuteNonQuery();


                command.Parameters.Clear();
                command.CommandText = $@"DELETE FROM Emails WHERE ContactID = @ID";
                command.Parameters.AddWithValue("@ID", ID);
                command.ExecuteNonQuery();

            }

            Connection.Close();

            DeleteContactUser(ID);
        }
    }
}