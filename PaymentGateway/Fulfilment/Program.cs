using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using StackExchange.Redis;
using Data.Repositories.Interfaces;
using Repository.Data.Implementations;
using Data.Repositories.Implementations;
using Domain.Models;
using Fulfilment.Services;
using Domain.Models.Entities;

namespace Fulfilment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

            var connectionMultiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!);
            var mongoSettings = builder.Configuration.GetSection("Mongo").Get<MongoSettings>();
            var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));

            builder.Services.AddSingleton(connectionMultiplexer);
            builder.Services.AddScoped<ICacheService, RedisCacheService>();
            builder.Services.AddSingleton(mongoClient);
            builder.Services.AddSingleton(mongoSettings!);
            builder.Services.AddScoped<IMerchantService, MongoDbMerchantService>();
            builder.Services.AddScoped<IPaymentService, MongoDbPaymentService>();
            builder.Services.AddScoped<IMerchantManagementService, MerchantManagementService>();
            builder.Services.AddScoped<IPaymentManagementService, PaymentManagementService>();
            builder.Services.AddHttpClient<IBankTransactionService, BankTransactionService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetSection("AquiringBank").GetValue<string>("APIUri")!);
            });

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();



            // HACKY SEED FOR DEMONSTRATIVE PURPOSES
            var testMerchant = new Merchant { MerchantId = "2134a701-3e9c-4c54-993f-ff2c3b41797f", MerchantName = "test merchant" };
            var collection = mongoClient.GetDatabase(mongoSettings.Database).GetCollection<Merchant>(mongoSettings.CollectionName);
            if (!collection.Find(_ => _.MerchantId == testMerchant.MerchantId).Any()) collection.InsertOne(testMerchant);
            //

            app.Run();
        }
    }
}