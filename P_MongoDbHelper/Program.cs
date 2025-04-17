using MongoDB.Bson;
using MongoDB.Driver;
using System;

class Program
{
    static void Main(string[] args)
    {
        var db = MongoDbHelper.Instance;
        string collection = "users";

        // INSERT example
        var newUser = new BsonDocument
        {
            { "username", "JohnDoe" },
            { "email", "john@example.com" },
            { "age", 25 }
        };
        db.Insert(collection, newUser);

        // SELECT example
        var filter = Builders<BsonDocument>.Filter.Eq("username", "JohnDoe");
        var result = db.Select(collection, filter);
        foreach (var doc in result)
        {
            Console.WriteLine(doc.ToJson());
        }

        // UPDATE example (only update "age" field)
        var update = Builders<BsonDocument>.Update.Set("age", 26);
        db.Update(collection, filter, update);

        /* // Select after update to verify

        // SELECT example
        var filter1 = Builders<BsonDocument>.Filter.Eq("username", "JohnDoe");
        var result1 = db.Select(collection, filter1);
        foreach (var doc in result1)
        {
            Console.WriteLine(doc.ToJson());
        }
        */

        // DELETE example
        db.Delete(collection, filter);
    }
}
