﻿using KiancaAPI.Context;
using KiancaAPI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KiancaAPI.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IDBContext _context;
        public PersonRepository(IDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Person>> Get()
        {
            return await _context
                            .Persons
                            .Find(_ => true)
                            .ToListAsync();
        }
        public Task<Person> Get(string id)
        {
            FilterDefinition<Person> filter = Builders<Person>.Filter.Eq(m => m.Id, id);
            return _context
                    .Persons
                    .Find<Person>(p => p.Id == id)
                    .FirstOrDefaultAsync();
        }
        public async Task Create(Person person)
        {
            await _context.Persons.InsertOneAsync(person);
        }
        public async Task<bool> Update(Person person)
        {
            ReplaceOneResult updateResult =
                await _context
                        .Persons
                        .ReplaceOneAsync(
                            filter: g => g.Id == person.Id,
                            replacement: person);
            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Person> filter = Builders<Person>.Filter.Eq(m => m.Id, id);
            DeleteResult deleteResult = await _context
                                                .Persons
                                              .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}
