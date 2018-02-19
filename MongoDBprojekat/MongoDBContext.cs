using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDBprojekat.Models;
using System.Threading.Tasks;

namespace MongoDBprojekat
{
    public class MongoDBContext : IDisposable
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public bool IsSSLEnabled { get; set; }
        private IMongoDatabase _database { get; set; }

        public MongoDBContext()
        {
            
        }

        public MongoDBContext(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~MongoDBContext() 
        {
           // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
           Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this); // kaze Garbage Collector - u da je objekat rucno "pokupljen"
        }
        #endregion

        public void Connect()
        {
            try
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));

                if (IsSSLEnabled)
                    settings.SslSettings = new SslSettings() { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };

                MongoClient client = new MongoClient(settings);
                _database = client.GetDatabase(DatabaseName);
            }

            catch(Exception ex)
            {
                throw new Exception("Cannot connect to the driver. ", ex);
            }
        }

        internal User GetUserByUsername(string username, string password)
        {
            var collection = _database.GetCollection<User>("users");
            var result = collection.Find(x => x.Username == username).ToList<User>();
            
            if(result != null)
            {
                User u = result.First();

                if(u.Password == password)
                    return u;

                return null;
            }

            return null;
        }

        internal void UpdateProfilePhoto(string id, string photoFileName)
        {
            if(_database != null)
            {
                var collection = _database.GetCollection<User>("users");
                var filter = Builders<User>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
                var update = Builders<User>.Update.Set(p => p.ProfilePicture, photoFileName);
                collection.FindOneAndUpdateAsync<User>(filter, update);
            }
            else
            {
                return;
            }
        }

        internal void UpdateUserUploads(UploadedFile uploadedFile, string Username)
        {
            var collection = _database.GetCollection<UploadedFile>("uploads");
            collection.InsertOne(uploadedFile);
            
            var collectionUser = _database.GetCollection<User>("users");
            User user = collectionUser.Find(x => x.Username == Username).ToList().First();

            if(user.Uploads == null)
            {
                user.Uploads = new List<UploadedFile>();
            }

            user.Uploads.Add(uploadedFile);
            var filter = Builders<User>.Filter.Eq("username", Username);
            var update = Builders<User>.Update.Set("uploads", user.Uploads);

            collectionUser.UpdateOne(filter, update);
        }

        public void InsertIntoUsers(User u)
        {
            var collection = _database.GetCollection<User>("users");
            collection.InsertOne(u);
        }

        internal User GetUserById(ObjectId id)
        {
            var collection = _database.GetCollection<User>("users");
            try
            {
                return collection.Find(x => x.Id == id).ToList().First();
            }
            catch(Exception e)
            {
                return null;
            }
        }

        internal void RemoveFromFiles(string fileId, string userId)
        {
            var tmp = ObjectId.Parse(fileId);

            var builder = Builders<UploadedFile>.Filter;
            var filter = builder.Eq(x => x.Id, tmp);
            var collection = _database.GetCollection<UploadedFile>("uploads");
            collection.DeleteOneAsync(filter);

            var userCollection = _database.GetCollection<User>("users");
            var userFilter = Builders<User>.Filter;
            var userFilterAndUpload = userFilter.And(
                Builders<User>.Filter.Eq(x => x.Id, ObjectId.Parse(userId)),
                Builders<User>.Filter.ElemMatch(x => x.Uploads, y => y.Id == ObjectId.Parse(fileId)));

            var updateUser = Builders<User>.Filter.ElemMatch(x => x.Uploads, y => y.Id == ObjectId.Parse(fileId));

            var test = userCollection.FindOneAndUpdateAsync(updateUser, null);
        }

        internal void UpdateUserDetails(string id, string firstname, string lastname, string username, string email, string password)
        {
            var userCollection = _database.GetCollection<User>("users");
            var userFilter = Builders<User>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
            var updateUser = Builders<User>.Update.Set(x => x.FirstName, firstname).
                Set(x=> x.LastName, lastname).
                Set(x => x.Username, username).
                Set(x=> x.Email, email).
                Set(x=> x.Password, password);


            userCollection.UpdateOneAsync(userFilter, updateUser);
        }

        internal void UpdateUserDetails(string id, string firstname, string lastname, string username, string email)
        {
            var userCollection = _database.GetCollection<User>("users");
            var userFilter = Builders<User>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
            var updateUser = Builders<User>.Update.Set(x => x.FirstName, firstname).
                Set(x => x.LastName, lastname).
                Set(x => x.Username, username).
                Set(x => x.Email, email);

            userCollection.UpdateOneAsync(userFilter, updateUser);
        }

        internal UploadedFile SearchUploadedFile(string searchID)
        {
            if(_database != null)
            {
                var collection = _database.GetCollection<UploadedFile>("uploads");
                var filter = Builders<UploadedFile>.Filter.Eq(x => x.Id, ObjectId.Parse(searchID));
                var res = collection.Find(filter).ToList().First();

                return res;
            }
            else
            {
                return new UploadedFile();
            }
        }

        internal void RemoveAccount(string id)
        {
            if(_database != null)
            {
                var collection = _database.GetCollection<User>("users");
                var filter = Builders<User>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
                collection.FindOneAndDeleteAsync(filter);
            }

            else
            {
                return;
            }
        }
        
        internal string GetGIFUrl(int id)
        {
            string url = "";
            if(_database != null)
            {
                var collection = _database.GetCollection<Gif>("gifs");
                var filter = Builders<Gif>.Filter.Eq(x => x.Id, id.ToString());

                url = collection.Find(filter).ToList().First().Url;
            }

            return url;
        }
    }
}


//var userCollection = _database.GetCollection<User>("users");
//var user = userCollection.Find(x => x.Id == ObjectId.Parse(userId)).ToList().First();

//int i = 0;

//            while(!(user.Uploads[i].Id == ObjectId.Parse(fileId)))
//            {
//                i++;
//            }

//            user.Uploads.RemoveAt(i);

//            var filter = Builders<User>.Filter.Eq("_id", userId);
//var update = Builders<User>.Update.Set("uploads", new BsonArray(user.Uploads));

//var result = userCollection.UpdateOne(filter, update);