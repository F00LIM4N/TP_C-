using GestionHotel.Apis;
using GestionHotel.Apis.Endpoints.Booking;
using System;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<SampleInjectionInterface, SampleInjectionImplementation>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapBookingsEndpoints();
app.Run();

class Program
{
    static void Main(string[] args)
    {
        var connString = "Server=127.0.0.1;Port=3306;Database=hotel_management;Uid=hotel_management;Pwd=Bakugo34970!;";
        using var conn = new MySqlConnection(connString);
        conn.Open();
        Console.WriteLine("Connected to MySQL database!");
    }
}