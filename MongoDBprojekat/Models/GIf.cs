using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBprojekat.Models
{
    [BsonDiscriminator("gifs")]
    public class Gif
    {
        public string Id { get; set; }
        [BsonElement("url")]
        public string Url { get; set; }
    }
}