using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace Data.SqlServer.Specific
{
    public class SqlServerCustomerDatabase : ICustomerDatabase
    {
        private string ConnectionString { get; }

        public SqlServerCustomerDatabase(ISqlServerConfiguration config)
        {
            ConnectionString = config.ConnectionString;
        }


        public async Task<Customer> GetCustomer(string emailAddress)
        {
            Customer customer = null;
            await using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var cmd = BuildGetCommand(emailAddress, connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
                customer = ReadCustomer(reader);

            connection.Close();

            return customer;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            var customerList = new List<Customer>();
            await using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var cmd = BuildGetAllCommand(connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
                customerList.Add(ReadCustomer(reader));

            connection.Close();

            return customerList;
        }

        public async Task SaveCustomer(Customer customer)
        {
            await using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var cmd = BuildSaveCommand(customer, connection);
            await cmd.ExecuteNonQueryAsync();

            connection.Close();
        }

        // NOTE: I DO NOT recommend the use of inline SQL in our C# code anywhere.
        // However, I wanted to keep the data access light and did it here anyways. 

        private static SqlCommand BuildGetCommand(string emailAddress, SqlConnection connection)
        {
            return new SqlCommand($"SELECT * FROM [dbo].[Customers] WHERE [EmailAddress] = '{emailAddress}'", connection);
        }

        private static SqlCommand BuildGetAllCommand(SqlConnection connection)
        {
            return new SqlCommand($"SELECT * FROM [dbo].[Customers]", connection);
        }

        private static SqlCommand BuildSaveCommand(Customer customer, SqlConnection connection)
        {
            return new SqlCommand($"INSERT INTO [dbo].[Customers] ([Guid], [FirstName], [LastName], [EmailAddress]) VALUES ('{customer.Id}', '{customer.FirstName}', '{customer.LastName}', '{customer.EmailAddress}')", connection);
        }


        private static Customer ReadCustomer(SqlDataReader reader)
        {
            var guid = new Guid(reader["Guid"].ToString());
            var firstName = reader["FirstName"].ToString();
            var lastName = reader["LastName"].ToString();
            var email = reader["EmailAddress"].ToString();

            return new Customer(guid, firstName, lastName, email);
        }
    }
}
