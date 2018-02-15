using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDBprojekat.Models
{
    public class UploadedFile
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}