using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDBprojekat.Models
{
    [BsonDiscriminator("users")]
    public class User
    {
        public ObjectId Id { get; set; }
        [BsonElement("firstname")]
        public string FirstName { get; set; }
        [BsonElement("lastname")]
        public string LastName { get; set; }
        [BsonElement("username")]
        public string Username { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("uploads")]
        public List<UploadedFile> Uploads { get; set; }

        public User(string firstname, string lastname, string email, string username, string password)
        {
            Id        = ObjectId.GenerateNewId();
            FirstName = firstname;
            LastName  = lastname;
            Email     = email;
            Username  = username;
            Password  = password;
            Uploads   = new List<UploadedFile>();
        }

        public User()
        {
        }
    }
}