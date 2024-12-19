/* This is a controller for doing CRUD ops on the Groupings collection of the database */
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using PowerBIEmbedApp.Models;

namespace PowerBIEmbedApp.Controllers;

public class GroupingController
{
    // Set up MongoClient and connect to database
    private const string MONGO_DB_CONN_STRING = "<YOUR MONGO DB CONNECTION STRING>";

    private static MongoClient mongoClient = new MongoClient(MONGO_DB_CONN_STRING);

    private static IMongoDatabase database = mongoClient.GetDatabase("PowerBIEmbedded-DB");

    // Get appropriate collection
    private static IMongoCollection<Grouping> collection = database.GetCollection<Grouping>("Groupings");

    // Create property to use as global state
    public List<Grouping> Groupings { get; set; } = collection.Find(new BsonDocument()).ToList().OrderBy(g => g.Name).ToList();

    // Event handler to invoke when state changes. Will trigger listening components to re-render
    public event EventHandler StateChangedHandler;

    // Method for invoking the event whenever the state changes
    private void StateHasChanged()
    {
        StateChangedHandler?.Invoke(this, EventArgs.Empty);
    }

    // Method to set groupings
    public void SetGroupings()
    {
        this.Groupings = collection.Find(new BsonDocument()).ToList().OrderBy(g => g.Name).ToList();
        StateHasChanged();
    }

    // Method to get all groupings
    public Task<List<Grouping>> GetAllGroupings()
    {
        List<Grouping> groupings = collection.Find(new BsonDocument()).ToList().OrderBy(g => g.Name).ToList();

        return Task.FromResult(groupings);
    }

    // Method to get a single grouping
    public Task<Grouping> GetGrouping(ObjectId id)
    {
        Grouping grouping = collection.Find(new BsonDocument()).ToList().Where(g => g.Id == id).ToList()[0];

        return Task.FromResult(grouping);
    }

    // Method to update a grouping
    public Task<ReplaceOneResult> UpdateGrouping(Grouping grouping)
    {
        FilterDefinition<Grouping> filter = Builders<Grouping>.Filter.Eq("_id", grouping.Id);
        ReplaceOneResult result = collection.ReplaceOne(filter, grouping);

        SetGroupings();

        return Task.FromResult(result);
    }

    // Method to create a grouping
    public Task<Grouping> AddGrouping(Grouping grouping)
    {
        collection.InsertOne(grouping);

        SetGroupings();

        return Task.FromResult(grouping);
    }

    // Method to delete a grouping
    public Task<DeleteResult> DeleteGrouping(ObjectId id)
    {
        FilterDefinition<Grouping> filter = Builders<Grouping>.Filter.Eq("_id", id);
        DeleteResult result = collection.DeleteOne(filter);

        SetGroupings();

        return Task.FromResult(result);
    }
}