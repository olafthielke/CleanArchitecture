using System.Linq;
using Amazon.SimpleEmail;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using BusinessLogic.UseCases;
using Data.Proxy;
using Data.Redis.Common;
using Data.Redis.Common.Interfaces;
using Data.Redis.Specific;
using Data.SqlServer.Specific;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Notification.Email.AWS;
using Notification.Email.AWS.Interfaces;
using Notification.Email.AWS.Services;
using Notification.Email.Interfaces;
using Notification.Email.Services;
using Notification.SMS;
using Microsoft.EntityFrameworkCore;
using Data.Postgres;

namespace Presentation.WebApi
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IGetAllCustomersUseCase, GetAllCustomersUseCase>();
            services.AddScoped<IRegisterCustomerUseCase, RegisterCustomerUseCase>();

            // IT'S LIKE LEGO FOR ADULTS!

            // Uncomment only one of the 6 numbered and separated blocks to radically alter 
            // the behaviour of the customer data persistence mechanisms, from a simple
            // in-memory database, up to a Redis cache / SQL database combination.

            // -----------------------------------------------------------------------------

            //// 1. *** REPO: In-Memory DB ***
            ConfigureInMemoryDatabases(services);

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

            //// 7. *** DATABASE: PostgresDB ***
            //// REPO
            //services.AddDbContext<DataContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Postgres-Database")));
            //services.AddScoped<ICustomerRepository, PostgresCustomerDatabase>();
            //services.AddScoped<IEmailTemplateRepository, PostgresEmailTemplateDatabase>();

            // -----------------------------------------------------------------------------
            //  Furthermore, options for ICustomerNotifier:

            services.AddScoped<ICustomerNotifier, CustomerEmailer>();
            services.AddScoped<IEmailConfiguration, HardcodedEmailConfiguration>();
            services.AddScoped<IPlaceholderReplacer, PlaceholderReplacer>();
            //services.AddScoped<IEmailer, NullEmailer>();

            services.Configure<AmazonConfiguration>(Configuration.GetSection("AWS"));
            services.AddScoped<IAmazonSimpleEmailServiceClientFactory, AmazonSimpleEmailServiceClientFactory>();

            services.AddScoped<IEmailer, AwsEmailer>();

            //var awsConfig = Configuration.GetAWSOptions();
            //services.AddDefaultAWSOptions(awsConfig);


            //services.AddAWSService<IAmazonSimpleEmailService>();

            //services.AddScoped<IAmazonConfiguration, HardcodedAmazonConfiguration>();

        }

        private static void ConfigureInMemoryDatabases(IServiceCollection services)
        {
            services.AddSingleton<ICustomerRepository, InMemoryCustomerDatabase>();

            services.AddSingleton<IEmailTemplateRepository, InMemoryEmailTemplateDatabase>();
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
    }
}
