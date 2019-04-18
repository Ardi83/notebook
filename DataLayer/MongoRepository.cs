using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using notebook.Models;

namespace notebook.DataLayer
{
    public class MongoRepository
    {
        private readonly IMongoDatabase _database;

        public MongoRepository(IOptions<Settings> settings)
        {
            try
            {
                var credential = MongoCredential.CreateCredential(settings.Value.Database, settings.Value.Username, settings.Value.Password);
                var server = new MongoServerAddress(settings.Value.ConnectionString);
                var setting = new MongoClientSettings
                {
                    Credential = credential,
                    Server = server
                };
                var client = new MongoClient(setting);
                if (client != null)
                    _database = client.GetDatabase(settings.Value.Database);
            }
            catch(Exception ex)
            {
                throw new Exception("Can't connect to db.", ex);
            }
        }

        public IMongoCollection<Note> Notes => _database.GetCollection<Note>("Notes");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
        public IMongoCollection<ApplicationUser> Users => _database.GetCollection<ApplicationUser>("Users");
    }
}