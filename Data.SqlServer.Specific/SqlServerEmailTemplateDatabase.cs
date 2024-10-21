using System.Data.SqlClient;
using System.Threading.Tasks;
using Notification.Email.Interfaces;
using Notification.Email.Models;

namespace Data.SqlServer.Specific
{
    public class SqlServerEmailTemplateDatabase(ISqlServerConfiguration config) : IEmailTemplateRepository
    {
        private string ConnectionString { get; } = config.ConnectionString;


        public async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            EmailTemplate template = null;
            await using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();

            var cmd = BuildGetCommand(templateName, connection);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
                template = ReadEmailTemplate(reader);

            return template;
        }

        // NOTE: I DO NOT recommend the use of inline SQL in our C# code anywhere.
        // However, I wanted to keep the data access light and did it here anyways. 

        private static SqlCommand BuildGetCommand(string templateName, SqlConnection connection)
        {
            return new SqlCommand($"SELECT * FROM [dbo].[EmailTemplates] WHERE [Name] = '{templateName}'", connection);
        }


        private static EmailTemplate ReadEmailTemplate(SqlDataReader reader)
        {
            var name = reader["Name"].ToString();
            var subject = reader["Subject"].ToString();
            var body = reader["Body"].ToString();

            return new EmailTemplate(name, subject, body);
        }
    }
}
