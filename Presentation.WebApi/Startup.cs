using System;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using BusinessLogic.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notification.Email.AWS;
using Notification.Email.AWS.Interfaces;
using Notification.Email.AWS.Services;
using Notification.Email.Interfaces;
using Notification.Email.Services;
using Microsoft.EntityFrameworkCore;
using Data.Postgres;
using Notification.Common;
using Notification.Common.Interfaces;
using Notification.Common.Services;
using Notification.Email.Models;
using Notification.SMS.Interfaces;
using Notification.SMS.Models;
using Notification.SMS.Services;
using Notification.SMS.Twilio;

namespace Presentation.WebApi
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // CLEAN ARCHITECTURE - IT'S LIKE LEGO FOR ADULTS!

            Configure_UseCases(services);

            Configure_CustomerRepository(services);

            Configure_CustomerNotifier(services);


            // Uncomment only one of the numbered and separated blocks to radically alter 
            // the behaviour of the customer data persistence mechanisms, from a simple
            // in-memory database, up to a Redis cache / SQL database combination.

            // -----------------------------------------------------------------------------

            //// 1. *** REPO: In-Memory DB ***
            //ConfigureInMemoryDatabases(services);

            // -----------------------------------------------------------------------------

            //// 2. *** REPO: JSON File ***
            //services.AddSingleton<ICustomerRepository, JsonCustomerFile>();

            // -----------------------------------------------------------------------------

            //// 3. *** REPO: SQL Server DB ***
            //var connectionString = Configuration.GetConnectionString("SqlServer-Database");
            //services.AddTransient<ISqlServerConfiguration>(s => new SqlServerConfiguration(connectionString));

            //services.AddScoped<ICustomerRepository, SqlServerCustomerDatabase>();
            //services.AddScoped<IEmailTemplateRepository, SqlServerEmailTemplateDatabase>();

            //services.AddScoped<ISqlServerConfiguration, HardcodedSqlServerConfiguration>();

            // -----------------------------------------------------------------------------

            //// 4. *** CACHE: NullCache | DATABASE: In-Memory DB ***
            //// REPO
            //services.AddScoped<ICustomerRepository, CachedCustomerRepository>();
            //// CACHE: Null (as in, Don't Cache!)
            //services.AddScoped<ICustomerCache, NullCustomerCache>();
            //// DATABASE: In-Memory DB Only
            //services.AddSingleton<ICustomerDatabase, InMemoryCustomerDatabase>();

            // -----------------------------------------------------------------------------

            //// 5. *** CACHE: In-Memory DB | DATABASE: JSON File ***
            //// REPO
            //services.AddScoped<ICustomerRepository, CachedCustomerRepository>();
            //// CACHE: In-Memory DB
            //services.AddScoped<ICustomerCache, InMemoryCustomerDatabase>();
            // DATABASE: JSON File
            //services.AddSingleton<ICustomerDatabase, JsonCustomerFile>();

            // -----------------------------------------------------------------------------

            // 6. *** CACHE: Redis | DATABASE: SQL Server DB ***
            //// REPO
            //services.AddScoped<ICustomerRepository, CachedCustomerRepository>();
            //// CACHE: Redis
            //services.AddScoped<ICustomerCache, RedisCustomerCache>();
            //services.AddScoped<IRedisConnector, RedisConnector>();
            //services.AddScoped<IRedisConfiguration, HardcodedRedisConfiguration>();
            //// DATABASE: SQL Server
            //services.AddScoped<ICustomerDatabase, SqlServerCustomerDatabase>();
            //services.AddScoped<IEmailTemplateRepository, SqlServerEmailTemplateDatabase>();
            //services.AddScoped<ISqlServerConfiguration, HardcodedSqlServerConfiguration>();

            // -----------------------------------------------------------------------------
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void Configure_UseCases(IServiceCollection services)
        {
            services.AddScoped<IGetAllCustomersUseCase, GetAllCustomersUseCase>();
            services.AddScoped<IRegisterCustomerUseCase, RegisterCustomerUseCase>();
        }

        // ICustomerRepository configuration
        private void Configure_CustomerRepository(IServiceCollection services)
        {
            //Configure_InMemoryDatabases(services);

            Configure_PostgresDatabase(services);
        }

        private static void Configure_InMemoryDatabases(IServiceCollection services)
        {
            services.AddSingleton<ICustomerRepository>(new InMemoryCustomerDatabase());

            var emailTemplate = new EmailTemplate(Templates.CustomerWelcome, "Welcome [[FirstName]] [[LastName]]!", "Hi [[FirstName]], \n\n It's good to have you with us! ...");
            var emailTemplates = new InMemoryEmailTemplateDatabase(emailTemplate);
            services.AddSingleton<IEmailTemplateRepository>(emailTemplates);

            var smsTemplate = new SmsTemplate(Templates.CustomerWelcome, "Hi. Thx for joining us, [[FirstName]]! ...");
            var smsTemplates = new InMemorySmsTemplateDatabase(smsTemplate);
            services.AddSingleton<ISmsTemplateRepository>(smsTemplates);
        }

        private void Configure_PostgresDatabase(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Postgres-Database")));

            services.AddScoped<ICustomerRepository, PostgresCustomerDatabase>();
            services.AddScoped<IEmailTemplateRepository, PostgresEmailTemplateDatabase>();
            services.AddScoped<ISmsTemplateRepository, PostgresSmsTemplateDatabase>();
        }


        // ICustomerNotifier configuration
        private void Configure_CustomerNotifier(IServiceCollection services)
        {
            //Configure_NullCustomerNotifier(services);

            //Configure_CustomerEmailer(services);

            Configure_CustomerSmsSender(services);
        }

        private static void Configure_NullCustomerNotifier(IServiceCollection services)
        {
            services.AddScoped<ICustomerNotifier, NullCustomerNotifier>();
        }

        private void Configure_CustomerEmailer(IServiceCollection services)
        {
            services.AddScoped<ICustomerNotifier, CustomerEmailer>();

            services.AddScoped<IPlaceholderReplacer, PlaceholderReplacer>();

            Configure_EmailConfiguration(services);

            Configure_Emailer(services);
        }

        private void Configure_EmailConfiguration(IServiceCollection services)
        {
            //Configure_HardcodedEmailConfiguration(services);

            Configure_DynamicEmailConfiguration(services);
        }

        private static void Configure_HardcodedEmailConfiguration(IServiceCollection services)
        {
            var config = new EmailConfiguration { FromAddress = "olaf@codecoach.co.nz" };

            services.AddSingleton(config);
        }

        private void Configure_DynamicEmailConfiguration(IServiceCollection services)
        {
            var config = new EmailConfiguration();

            Configuration.Bind("Email", config);

            services.AddSingleton(config);
        }

        private void Configure_Emailer(IServiceCollection services)
        {
            //Configure_NullEmailer(services);

            Configure_AwsEmailer(services);

            //Configure_SmtpEmailer(services); // TODO
        }

        private static void Configure_NullEmailer(IServiceCollection services)
        {
            services.AddScoped<IEmailer, NullEmailer>();
        }

        private void Configure_AwsEmailer(IServiceCollection services)
        {
            services.AddScoped<IEmailer, AwsEmailer>();

            services.AddScoped<IAmazonSimpleEmailServiceClientFactory, AmazonSimpleEmailServiceClientFactory>();

            Configure_AmazonConfiguration(services);
        }

        private void Configure_AmazonConfiguration(IServiceCollection services)
        {
            var config = new AmazonConfiguration();

            Configuration.Bind("AWS", config);

            services.AddSingleton(config);
        }

        private void Configure_CustomerSmsSender(IServiceCollection services)
        {
            services.AddScoped<ICustomerNotifier, CustomerSmsSender>();

            services.AddScoped<IPlaceholderReplacer, PlaceholderReplacer>();

            Configure_SmsConfiguration(services);

            Configure_SmsSender(services);
        }

        private void Configure_SmsConfiguration(IServiceCollection services)
        {
            //Configure_HardcodedSmsConfiguration(services);

            //Configure_DynamicSmsConfiguration(services);

            Configure_DynamicWhatsAppConfiguration(services);
        }

        private static void Configure_HardcodedSmsConfiguration(IServiceCollection services)
        {
            var config = new SmsConfiguration { FromNumber = "+13392012134" };

            services.AddSingleton(config);
        }

        private void Configure_DynamicSmsConfiguration(IServiceCollection services)
        {
            var config = new SmsConfiguration();

            Configuration.Bind("SMS", config);

            services.AddSingleton(config);
        }

        private void Configure_DynamicWhatsAppConfiguration(IServiceCollection services)
        {
            var config = new SmsConfiguration();

            Configuration.Bind("WhatsApp", config);

            services.AddSingleton(config);
        }

        private void Configure_SmsSender(IServiceCollection services)
        {
            //Configure_NullSmsSender(services);

            //Configure_TwilioSmsSender(services);

            Configure_TwilioWhatsAppSender(services);
        }

        private void Configure_NullSmsSender(IServiceCollection services)
        {
            services.AddScoped<ISmsSender, NullSmsSender>();
        }

        private void Configure_TwilioSmsSender(IServiceCollection services)
        {
            services.AddScoped<ISmsSender, TwilioSmsSender>();

            Configure_TwilioConfiguration(services);
        }

        private void Configure_TwilioWhatsAppSender(IServiceCollection services)
        {
            services.AddScoped<ISmsSender, TwilioWhatsAppSender>();

            Configure_TwilioConfiguration(services);
        }

        private void Configure_TwilioConfiguration(IServiceCollection services)
        {
            var config = new TwilioConfiguration();

            Configuration.Bind("Twilio", config);

            services.AddSingleton(config);
        }
    }
}
