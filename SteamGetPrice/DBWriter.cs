using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;

namespace SteamGetPrice
{
    class DBWriter
    {
        string connectionString = "";
        MongoClient client;
        IMongoDatabase database;
        public DBWriter(string connectionString, string databaseName)
        {
            this.connectionString = connectionString;
            client = new MongoClient(this.connectionString);
            database = client.GetDatabase(databaseName);
        }
        public void Insert(string jsonStr)
        {
            using (JsonReader jReader = new JsonReader(jsonStr))
            {
                var context = BsonDeserializationContext.CreateRoot(jReader);
                BsonDocument doc = BsonDocumentSerializer.Instance.Deserialize(context);
                var collection = database.GetCollection<BsonDocument>("prices");
                collection.InsertOneAsync(doc);

            }

            
        }
    }
}
