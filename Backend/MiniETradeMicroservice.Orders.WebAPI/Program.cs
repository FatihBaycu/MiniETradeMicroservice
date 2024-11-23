using MiniETradeMicroservice.Orders.WebAPI.Context;
using MiniETradeMicroservice.Orders.WebAPI.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.CSharpLegacy));
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.Configure<List<ApiUrl>>(builder.Configuration.GetSection("ApiUrls"));

builder.Services.AddSingleton<MongoDBContext>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
