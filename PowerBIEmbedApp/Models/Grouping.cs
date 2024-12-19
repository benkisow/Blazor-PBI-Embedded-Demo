using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PowerBIEmbedApp.Models;

public class Grouping
{
    public Grouping(string name, string icon)
        : this(name, icon, new ObjectId[0])
    {
    }
    public Grouping(string name, string icon, ObjectId[] subgroupingIds)
    {
        Name = name;
        Icon = icon;
        SubgroupingIds = subgroupingIds;
    }

    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public ObjectId[] SubgroupingIds { get; set; }
}
