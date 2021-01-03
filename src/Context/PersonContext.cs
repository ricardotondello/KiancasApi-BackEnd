using System;
using System.Collections.Generic;
using System.Linq;
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

            Seed(config.IsDevelopment);
        }

        public void Seed(bool IsDevelopment)
        {
            Console.WriteLine("IsDevelopment", IsDevelopment);
            if (!IsDevelopment) return;
            Person.DeleteMany(FilterDefinition<Person>.Empty);
            
            var list = Enumerable
                .Range(0, 100)
                .Select(i => new Person
                {
                    Name = nameof(Person) + i, 
                    Photo = Guid.NewGuid().ToString(),
                    BirthDate = DateTime.Today
                })
                .ToArray();
            
            Person.InsertMany(list);
        }
        public IMongoCollection<Person> Person => _db.GetCollection<Person>("person");
    }
}
