using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Drawing;

namespace PowerBIEmbedApp.Models;

public class Subgrouping
{
    public Subgrouping(string name)
        : this(name, new ObjectId[0])
    {
    }
    public Subgrouping(string name, ObjectId[] groupingItems)
    {
        Name = name;
        GroupingItems = groupingItems;
    }

    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public ObjectId[] GroupingItems { get; set; }
}