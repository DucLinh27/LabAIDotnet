using LabAI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;


namespace LabAI.Data
{

    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDBConnection"));
            _database = client.GetDatabase("comp1682");
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("products");
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("orders");
    }
}