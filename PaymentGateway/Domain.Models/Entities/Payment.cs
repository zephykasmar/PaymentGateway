using Domain.Models.DTOs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    public class Payment
    {
        public ObjectId Id { get; private set; }
        [BsonElement("transaction_id")]
        public string TransactionId { get; set; } = string.Empty;
        [BsonElement("merchant_id")]
        public string MerchantId { get; set; } = string.Empty;
        [BsonElement("cardholders_name")]
        public string CardholdersName { get; set; }
        [BsonElement("pan")]
        public string PAN { get; set; }
        [BsonElement("expiry")]
        [RegularExpression("\\d\\d\\/\\d\\d")]
        public string Expiry { get; set; }
        [BsonElement("amount")]
        public decimal Amount { get; set; }
        [BsonElement("currency_code")]
        public string CurrencyCode { get; set; }
    }
}
