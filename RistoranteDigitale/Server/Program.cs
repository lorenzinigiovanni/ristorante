using Microsoft.EntityFrameworkCore;
using RistoranteDigitaleServer.Hubs;
using RistoranteDigitaleServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationContext>(
    opt => opt.UseNpgsql("Host=localhost;Database=RistoranteDigitale;Username=RistoranteDigitale;Password=RistoranteDigitale")
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<OrdersHub>("/orders");
app.MapHub<ItemsHub>("/items");

app.Run();
