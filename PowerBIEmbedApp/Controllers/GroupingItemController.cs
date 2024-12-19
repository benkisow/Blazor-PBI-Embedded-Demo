/* This is a controller for doing CRUD ops on the GroupingItems collection of the database */
using MongoDB.Bson;
using MongoDB.Driver;
using PowerBIEmbedApp.Models;

namespace PowerBIEmbedApp.Controllers;

public class GroupingItemController
{
    // Set up MongoClient and connect to database
    private const string MONGO_DB_CONN_STRING = "<YOUR MONGO DB CONNECTION STRING>";

    private static MongoClient mongoClient = new MongoClient(MONGO_DB_CONN_STRING);

    private static IMongoDatabase database = mongoClient.GetDatabase("PowerBIEmbedded-DB");

    // Get appropriate collection
    private static IMongoCollection<GroupingItem> collection = database.GetCollection<GroupingItem>("GroupingItems");

    // Create property to use as global state
    public List<GroupingItem> GroupingItems { get; set; } = collection.Find(new BsonDocument()).ToList().OrderBy(g => g.Name).ToList();

    // Event handler to invoke when state changes. Will trigger listening components to re-render
    public event EventHandler StateChangedHandler;

    // Method for invoking the event whenever the state changes
    private void StateHasChanged()
    {
        StateChangedHandler?.Invoke(this, EventArgs.Empty);
    }

    // Method to set grouping items
    public void SetGroupingItems()
    {
        this.GroupingItems = collection.Find(new BsonDocument()).ToList().OrderBy(g => g.Name).ToList();
        StateHasChanged();
    }

    // Method to get all grouping items
    public Task<List<GroupingItem>> GetAllGroupingItems()
    {
        List<GroupingItem> groupingItems = collection.Find(new BsonDocument()).ToList().OrderBy(g => g.Name).ToList();

        return Task.FromResult(groupingItems);
    }

    // Method to get a single grouping item
    public Task<GroupingItem> GetGroupingItemItem(ObjectId id)
    {
        GroupingItem groupingItem = collection.Find(new BsonDocument()).ToList().Where(g => g.Id == id).ToList()[0];

        return Task.FromResult(groupingItem);
    }

    // Method to update a grouping item
    public Task<ReplaceOneResult> UpdateGroupingItem(GroupingItem groupingItem)
    {
        FilterDefinition<GroupingItem> filter = Builders<GroupingItem>.Filter.Eq("_id", groupingItem.Id);
        ReplaceOneResult result = collection.ReplaceOne(filter, groupingItem);

        SetGroupingItems();

        return Task.FromResult(result);
    }

    // Method to create a grouping item
    public Task<GroupingItem> AddGroupingItem(GroupingItem groupingItem)
    {
        collection.InsertOne(groupingItem);

        SetGroupingItems();

        return Task.FromResult(groupingItem);
    }

    // Method to delete a grouping item
    public Task<DeleteResult> DeleteGroupingItem(ObjectId id)
    {
        FilterDefinition<GroupingItem> filter = Builders<GroupingItem>.Filter.Eq("_id", id);
        DeleteResult result = collection.DeleteOne(filter);

        SetGroupingItems();

        return Task.FromResult(result);
    }
}
