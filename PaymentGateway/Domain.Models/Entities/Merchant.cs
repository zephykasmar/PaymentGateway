using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.Entities
{
    public class Merchant
    {
        public ObjectId Id { get; set; }
        [BsonElement("merchant_id")]
        public string MerchantId { get; set; }
        [BsonElement("merchant_name")]
        public string MerchantName { get; set; }
    }
}