using System;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.UseCases;
using Data.Proxy;
using Data.Redis.Common;
using Data.Redis.Specific;
using Data.SqlServer.Specific;
using Notification.Email;
using Notification.Email.Aws;
using Notification.Email.Interfaces;

namespace Presentation.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var registration = CreateCustomerRegistration();

            var useCase = SetupRegisterCustomerUseCase();

            try
            {
                var customer = useCase.RegisterCustomer(registration).Result;
                LogCustomer(customer);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            Console.ReadLine();
        }


        private static CustomerRegistration CreateCustomerRegistration()
        {
            // Customer Registrations
            //return new CustomerRegistration("Fred", "Flintstone", "fred@flintstones.net");
            return new CustomerRegistration("Barney", "Rubble", "barney@rubbles.rock");
            //return new CustomerRegistration("Wilma", "Flintstone", "wilma@flintstones.net");
            //return new CustomerRegistration("Bambam", "Rubble", "bambam@rubbles.rock");
        }

        private static RegisterCustomerUseCase SetupRegisterCustomerUseCase()
        {
            var repository = SetupCustomerRepository();
            var notifier = SetupCustomerNotifier();
            return new RegisterCustomerUseCase(repository, notifier);
        }

        private static ICustomerRepository SetupCustomerRepository()
        {
            var database = SetupSqlServerCustomerDatabase();
            var cache = SetupRedisCustomerCache();
            //var cache = new NullCache();
            return SetupCachedCustomerRepository(database, cache);
        }

        private static ICustomerCache SetupRedisCustomerCache()
        {
            var config = new RedisConfiguration();
            var connector = new RedisConnector(config);
            return new RedisCustomerCache(connector);
        }

        private static ICustomerDatabase SetupSqlServerCustomerDatabase()
        {
            var config = new SqlServerConfiguration();
            return new SqlServerCustomerDatabase(config);
        }

        private static ICustomerRepository SetupCachedCustomerRepository(ICustomerDatabase database, ICustomerCache cache)
        {
            return new CachedCustomerRepository(database, cache);
        }


        private static ICustomerNotifier SetupCustomerNotifier()
        {
            var emailer = SetupEmailer();
            var emailConfig = SetupEmailConfiguration();
            var emailTemplates = SetupEmailTemplateRepository();
            return SetupCustomerEmailer(emailer, emailConfig, emailTemplates);
        }

        private static IEmailer SetupEmailer()
        {
            return new AwsEmailer();
        }

        private static IEmailConfiguration SetupEmailConfiguration()
        {
            return null;
        }

        private static IEmailTemplateRepository SetupEmailTemplateRepository()
        {
            return null;
        }

        private static ICustomerNotifier SetupCustomerEmailer(IEmailer emailer,
            IEmailConfiguration emailConfig, 
            IEmailTemplateRepository emailTemplates)
        {
            return new CustomerEmailer(emailer, emailConfig, emailTemplates);
        }

        private static void LogCustomer(Customer customer)
        {
            Console.WriteLine($"Customer '{customer.FirstName} {customer.LastName}' has been registered.");
        }

        private static void LogError(Exception ex)
        {
            Console.WriteLine(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
        }
    }
}
