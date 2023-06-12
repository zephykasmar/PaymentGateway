using Analytics.Services;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Domain.Models;
using MongoDB.Driver;
using Repository.Data.Implementations;
using StackExchange.Redis;

namespace Analytics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var connectionMultiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!);
            var mongoSettings = builder.Configuration.GetSection("Mongo").Get<MongoSettings>();
            var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));

            builder.Services.AddSingleton(connectionMultiplexer);
            builder.Services.AddScoped<ICacheService, RedisCacheService>();
            builder.Services.AddSingleton(mongoClient);
            builder.Services.AddSingleton(mongoSettings!);
            builder.Services.AddScoped<IPaymentsAnalyticsService, PaymentsAnalyticsService>();
            builder.Services.AddScoped<IMerchantService, MongoDbMerchantService>();
            builder.Services.AddScoped<IPaymentService, MongoDbPaymentService>();
          
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}