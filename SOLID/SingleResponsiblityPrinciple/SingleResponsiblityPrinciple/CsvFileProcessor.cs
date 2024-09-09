using SingleResponsiblityPrinciple.BAL;
using SingleResponsiblityPrinciple.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsiblityPrinciple
{
    public class CsvFileProcessor
    {
        public void Process(IContactDataProvider cdp, IContactParser cp, IContactWriter cw)
        {
            var providedData = cdp.Read();
            var parsedData= cp.Parse(providedData);
            cw.Write(parsedData);
        }
    }
    public class CSVContactDataProvider : IContactDataProvider
    {
        private readonly string _filename;

        public CSVContactDataProvider(string filename)
        {
            _filename = filename;
        }

        public string Read()
        {
            TextReader tr = new StreamReader(_filename);
            tr.ReadToEnd();
            tr.Close();
            return tr.ToString();
        }
    }

    public class CSVContactParser : IContactParser
    {
        public IList<ContactDTO> Parse(string csvData)
        {
            IList<ContactDTO> contacts = new List<ContactDTO>();
            string[] lines = csvData.Split(new string[] { @"\r\l" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string[] columns = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                var contact = new ContactDTO
                {
                    FirstName = columns[0],
                    LastName = columns[1],
                    Email = columns[2]
                };
                contacts.Add(contact);
            }

            return contacts;
        }
    }

    public class ADOContactWriter : IContactWriter
    {
        public void Write(IList<ContactDTO> contacts)
        {
            var conn = new SqlConnection("server=(local);integrated security=sspi;database=SRP");
            conn.Open();
            foreach (var contact in contacts)
            {
                var command = conn.CreateCommand();
                command.CommandText = "INSERT INTO People (FirstName, LastName, Email) VALUES (@FirstName, @LastName, @Email)";
                command.Parameters.AddWithValue("@FirstName", contact.FirstName);
                command.Parameters.AddWithValue("@LastName", contact.LastName);
                command.Parameters.AddWithValue("@Email", contact.Email);
                command.ExecuteNonQuery();
            }
            conn.Close();

        }
    }

}
