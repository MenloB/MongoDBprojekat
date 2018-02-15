using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Core;

namespace MongoDBprojekat.DBContext
{
    public class MongoDBContext
    {
        public static string ConnectionString { get; set; }
        public static string DatabaseName { get; set; }
        public static bool IsSSLEnabled { get; set; }
        private IMongoDatabase _database { get; }

        public MongoDBContext()
        {
            try
            {
                MongoClientSettings mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));

                if(IsSSLEnabled)
                    mongoClientSettings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };

                var mongoClient = new MongoClient(mongoClientSettings);
                _database = mongoClient.GetDatabase(DatabaseName);

            }

            catch(Exception)
            {
                throw new Exception("Cannot connect to database specified");
            }
        }

        public void InsertUser(User u)
        {
            var collection = _database.GetCollection<Models.User>("users");
            collection.InsertOne(u);
        }
        
    }
}