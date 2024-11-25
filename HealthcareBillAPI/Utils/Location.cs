

using MongoDB.Bson.Serialization.Attributes;

namespace HealthcareBillAPI.Utils
{
    public class Location
    {
        [BsonElement(nameof(Address))]
        public string? Address { get; set; }
        [BsonElement(nameof(City))]
        public string? City { get; set; }
        [BsonElement(nameof(Country))]
        public string? Country { get; set; }
    }
}
