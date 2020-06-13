using KiancaAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace KiancaAPI.Services
{
    public class PersonService
    {
        private readonly IMongoCollection<Person> _persons;

        public PersonService(IPersonstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _persons = database.GetCollection<Person>(settings.PersonCollectionName);
        }

        public List<Person> Get() =>
            _persons.Find(book => true).ToList();

        public Person Get(string id) =>
            _persons.Find<Person>(p => p.Id == id).FirstOrDefault();

        public Person Create(Person p)
        {
            _persons.InsertOne(p);
            return p;
        }

        public void Update(string id, Person personIn) =>
            _persons.ReplaceOne(p => p.Id == id, personIn);

        public void Remove(Person personIn) =>
            _persons.DeleteOne(p => p.Id == personIn.Id);

        public void Remove(string id) =>
            _persons.DeleteOne(p => p.Id == id);
    }
}
