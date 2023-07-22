using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using Data.Redis.Interfaces;

namespace Data.Redis
{
    public class RedisCustomerCache : RedisCache<Customer>, ICustomerCache
    {
        public RedisCustomerCache(IRedisConnector connector)
            : base(connector)
        { }

        public async Task<Customer> GetCustomer(string emailAddress)
        {
            var cacheKey = BuildCustomerCacheKey(emailAddress);
            return await Get(cacheKey);
        }

        public async Task SaveCustomer(Customer customer)
        {
            var cacheKey = BuildCustomerCacheKey(customer.EmailAddress);
            await Save(cacheKey, customer);
        }

        private static string BuildCustomerCacheKey(string emailAddress)
        {
            return $"customer_{emailAddress}".ToLowerInvariant();
        }
    }
}
