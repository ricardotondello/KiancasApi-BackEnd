using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KiancaAPI.Models;
using MongoDB.Driver;

namespace KiancaAPI.Context
{
    public interface IDBContext
    {
        IMongoCollection<Person> Person { get; }
    }
}
