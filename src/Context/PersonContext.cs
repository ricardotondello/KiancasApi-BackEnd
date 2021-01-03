using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KiancaAPI.Models;
using MongoDB.Driver;

namespace KiancaAPI.Context
{
    public class PersonContext : IDBContext
    {
        private readonly IMongoDatabase _db;

        public PersonContext(MongoDBConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);
        }
        public IMongoCollection<Person> Person => _db.GetCollection<Person>("person");
    }
}
