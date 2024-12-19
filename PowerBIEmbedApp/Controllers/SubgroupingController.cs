/* This is a controller for doing CRUD ops on the Subgroupings collection of the database */
using MongoDB.Bson;
using MongoDB.Driver;
using PowerBIEmbedApp.Models;

namespace PowerBIEmbedApp.Controllers;

public class SubgroupingController
{
    // Set up MongoClient and connect to database
    private const string MONGO_DB_CONN_STRING = "<YOUR MONGO DB CONNECTION STRING>";

    private static MongoClient mongoClient = new MongoClient(MONGO_DB_CONN_STRING);

    private static IMongoDatabase database = mongoClient.GetDatabase("PowerBIEmbedded-DB");

    // Get appropriate collection
    private static IMongoCollection<Subgrouping> collection = database.GetCollection<Subgrouping>("Subgroupings");

    public List<Subgrouping> Subgroupings { get; set; } = collection.Find(new BsonDocument()).ToList().OrderBy(s => s.Name).ToList();

    // Method to set subgroupings
    public void SetSubgroupings()
    {
        this.Subgroupings = collection.Find(new BsonDocument()).ToList().OrderBy(s => s.Name).ToList();
    }

    // Method to get all subgroupings
    public Task<List<Subgrouping>> GetAllSubgroupings()
    {
        List<Subgrouping> subgroupings = collection.Find(new BsonDocument()).ToList().OrderBy(s => s.Name).ToList();

        return Task.FromResult(subgroupings);
    }

    // Method to get a single subgrouping
    public Task<Subgrouping> GetSubgrouping(ObjectId id)
    {
        Subgrouping subgrouping = collection.Find(new BsonDocument()).ToList().Where(s => s.Id == id).ToList()[0];

        return Task.FromResult(subgrouping);
    }

    // Method to update a subgrouping
    public Task<ReplaceOneResult> UpdateSubgrouping(Subgrouping subgrouping)
    {
        FilterDefinition<Subgrouping> filter = Builders<Subgrouping>.Filter.Eq("_id", subgrouping.Id);
        ReplaceOneResult result = collection.ReplaceOne(filter, subgrouping);

        SetSubgroupings();

        return Task.FromResult(result);
    }

    // Method to create a subgrouping
    public Task<Subgrouping> AddSubgrouping(Subgrouping subgrouping)
    {
        collection.InsertOne(subgrouping);

        SetSubgroupings();

        return Task.FromResult(subgrouping);
    }

    // Method to delete a subgrouping
    public Task<DeleteResult> DeleteSubgrouping(ObjectId id)
    {
        FilterDefinition<Subgrouping> filter = Builders<Subgrouping>.Filter.Eq("_id", id);
        DeleteResult result = collection.DeleteOne(filter);

        SetSubgroupings();

        return Task.FromResult(result);
    }
}
