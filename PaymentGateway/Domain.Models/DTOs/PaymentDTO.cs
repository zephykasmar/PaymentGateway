using Domain.Models.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Domain.Models.DTOs
{
    public class PaymentDTO
    {
        public string TransactionId { get; set; } = string.Empty;
        [BsonElement("merchant_id")]
        public string MerchantId { get; set; } = string.Empty;
        [BsonElement("cardholders_name")]
        public string CardholdersName { get; set; }
        [BsonElement("pan")]
        [RegularExpression("\\d{4}-\\d{4}-\\d{4}-\\d{4}")]
        public string PAN { get; set; }
        [BsonElement("expiry")]
        [RegularExpression("\\d\\d\\/\\d\\d")]
        public string Expiry { get; set; }
        [BsonElement("cvv")]
        [BsonIgnore]
        public string CVV { get; set; }
        [BsonElement("amount")]
        public decimal Amount { get; set; }
        [BsonElement("currency_code")]
        public string CurrencyCode { get; set; }
    }
}
