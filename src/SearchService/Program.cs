using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Data;
using SearchService.Models;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionServiceHttpClient>();

var app = builder.Build();


app.UseAuthorization();

app.MapControllers();
try
{
    await DBInitializer.InitDb(app);
}
catch (Exception e)
{
    System.Console.WriteLine(e.Message);
}

app.Run();
