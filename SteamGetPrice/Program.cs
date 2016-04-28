using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using FluentAssertions;

namespace SteamGetPrice
{
    class Program
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        static void Main(string[] args)
        {
            //_client = new MongoClient();
            //_database = _client.GetDatabase("test");
            //try {
            //    var task2 = TestQuery();
            //    task2.Wait();
            //}catch(Exception e)
            //{
            //    Console.Write(e.Message);
            //}
            PriceCrawler crawler = new PriceCrawler();
            string jsonStr = crawler.Run();
            DBWriter writer = new DBWriter("mongodb://localhost", "test");
            writer.Insert(jsonStr);
            Console.Read();
        }

        static async Task TestInsert()
        {
            var document = new BsonDocument
            {
                {
                    "address" , new BsonDocument
        {
            { "street", "2 Avenue" },
            { "zipcode", "10075" },
            { "building", "1480" },
            { "coord", new BsonArray { 73.9557413, 40.7720266 } }
        }
                },
    { "borough", "Manhattan" },
    { "cuisine", "Italian" },
    {
                    "grades", new BsonArray
        {
            new BsonDocument
            {
                { "date", new DateTime(2014, 10, 1, 0, 0, 0, DateTimeKind.Utc) },
                { "grade", "A" },
                { "score", 11 }
            },
            new BsonDocument
            {
                { "date", new DateTime(2014, 1, 6, 0, 0, 0, DateTimeKind.Utc) },
                { "grade", "B" },
                { "score", 17 }
            }
        }
    },
    { "name", "Vella" },
    { "restaurant_id", "41704620" }
            };

            var collection = _database.GetCollection<BsonDocument>("restaurants");
            await collection.InsertOneAsync(document);
            Console.WriteLine("insert already");
        }
        static async Task TestQuery()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var filter = new BsonDocument();
            var count = 0;
            using (var cursor = await collection.FindAsync(filter))
            {
                while(await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach(var document in batch)
                    {
                        count++;
                    }
                }
            }
            Console.WriteLine(count);
            //count.Should().Be(25349,"it should be 25349");
        }

        static async Task TestQueryFilter()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var filter = Builders<BsonDocument>.Filter.Eq("borough", "Manhattan");
            var result = await collection.Find(filter).ToListAsync();
            
            foreach (var entry in result)
            {
                entry.ToString();
            }
        }
    }
}
