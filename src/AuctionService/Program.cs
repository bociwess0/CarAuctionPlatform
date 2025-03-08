using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();


app.UseAuthorization();

app.MapControllers();

try
{
    DBInitializer.InitDb(app);
}
catch (System.Exception e)
{
    System.Console.WriteLine(e.Message);    
}

app.Run();
