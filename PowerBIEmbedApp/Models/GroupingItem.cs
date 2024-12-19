using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PowerBIEmbedApp.Enums;

namespace PowerBIEmbedApp.Models;

public class GroupingItem
{
    public GroupingItem(string powerBiId, string name, ItemType itemType)
    {
        this.PowerBiId = powerBiId;
        this.Name = name;
        this.ItemType = itemType;
    }

    [BsonId]
    public ObjectId Id { get; set; }
    public string PowerBiId { get; set; }
    public string Name { get; set; }
    public ItemType ItemType { get; set; }
}