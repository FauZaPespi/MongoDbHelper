using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

public sealed class MongoDbHelper
{
    private static readonly Lazy<MongoDbHelper> lazyInstance = new Lazy<MongoDbHelper>(() => new MongoDbHelper());

    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly string _connectionString = "mongodb://localhost:27017"; // Change if needed
    private readonly string _databaseName = "YourDatabaseName"; // Change to your DB name

    public static MongoDbHelper Instance => lazyInstance.Value;

    private MongoDbHelper()
    {
        _client = new MongoClient(_connectionString);
        _database = _client.GetDatabase(_databaseName);
    }

    // ✅ Health check
    public bool IsDbHealthy()
    {
        try
        {
            _client.ListDatabaseNames(); // Will throw if not connected
            return true;
        }
        catch
        {
            return false;
        }
    }

    // ✅ SELECT
    public List<BsonDocument> Select(string collectionName, FilterDefinition<BsonDocument> filter)
    {
        if (!IsDbHealthy())
        {
            Console.WriteLine("Database not reachable: SELECT aborted.");
            return new List<BsonDocument>();
        }

        var collection = _database.GetCollection<BsonDocument>(collectionName);
        return collection.Find(filter).ToList();
    }

    // ✅ INSERT
    public void Insert(string collectionName, BsonDocument document)
    {
        if (!IsDbHealthy())
        {
            Console.WriteLine("Database not reachable: INSERT aborted.");
            return;
        }

        var collection = _database.GetCollection<BsonDocument>(collectionName);
        collection.InsertOne(document);
    }

    // ✅ UPDATE
    public void Update(string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update)
    {
        if (!IsDbHealthy())
        {
            Console.WriteLine("Database not reachable: UPDATE aborted.");
            return;
        }

        var collection = _database.GetCollection<BsonDocument>(collectionName);
        collection.UpdateMany(filter, update);
    }

    // ✅ DELETE
    public void Delete(string collectionName, FilterDefinition<BsonDocument> filter)
    {
        if (!IsDbHealthy())
        {
            Console.WriteLine("Database not reachable: DELETE aborted.");
            return;
        }

        var collection = _database.GetCollection<BsonDocument>(collectionName);
        collection.DeleteMany(filter);
    }
}
