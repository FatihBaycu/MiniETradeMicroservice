using Microsoft.Extensions.Options;
using MiniETradeMicroservice.Orders.WebAPI.Models;
using MongoDB.Driver;

namespace MiniETradeMicroservice.Orders.WebAPI.Context
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;
        public MongoDBContext(IOptions<MongoDBSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
