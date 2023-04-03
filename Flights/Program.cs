using Microsoft.OpenApi.Models;
using Flights.Data;
using Flights.Domain.Entities;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

//Add db context
builder.Services.AddDbContext<Entities>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Flights")));

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen(c =>
{
    c.DescribeAllParametersInCamelCase();

    c.AddServer(new OpenApiServer
    {
        Description = "Development Server",
        Url = "https://localhost:7224"
    });

    c.CustomOperationIds(
        e => $"{e.ActionDescriptor.RouteValues["action"] + e.ActionDescriptor.RouteValues["controller"]}"
        );
});

builder.Services.AddScoped<Entities>();

var app = builder.Build();

var entities = app.Services.CreateScope().ServiceProvider.GetService<Entities>();

entities.Database.EnsureCreated();

var random = new Random();

if (!entities.Flights.Any())
{

    Flight[] flightsToSeed = new Flight[]{
    new (
                     Guid.NewGuid(),
                     "American Airlines",
                     random.Next(90,5000).ToString(),
                     new TimePlace("Los Angeles", DateTime.Now.AddHours(random.Next(1,3))),
                     new TimePlace("Istanbul", DateTime.Now.AddHours(random.Next(4,10))),
                     2
                     ),
                 new (
                     Guid.NewGuid(),
                     "Deutsche BA",
                     random.Next(90,5000).ToString(),
                     new TimePlace("Munchen", DateTime.Now.AddHours(random.Next(1,3))),
                     new TimePlace("Schiphol", DateTime.Now.AddHours(random.Next(4,10))),
                     random.Next(1,853)
                     ),
                 new (
                     Guid.NewGuid(),
                     "Blasiq Air",
                     random.Next(90,5000).ToString(),
                     new TimePlace("Amsterdam", DateTime.Now.AddHours(random.Next(1,3))),
                     new TimePlace("Glasgow", DateTime.Now.AddHours(random.Next(4,10))),
                     random.Next(1,853)
                     ),
                 new (
                     Guid.NewGuid(),
                     "Adria Airways",
                     random.Next(90,5000).ToString(),
                     new TimePlace("Ljubljana", DateTime.Now.AddHours(random.Next(1,3))),
                     new TimePlace("Warsaw", DateTime.Now.AddHours(random.Next(4,10))),
                     random.Next(1,853)
                     ),
                 new (
                     Guid.NewGuid(),
                "ABA Air",
                     random.Next(90,5000).ToString(),
                     new TimePlace("Praha Ruzyne", DateTime.Now.AddHours(random.Next(1,3))),
                     new TimePlace("Paris", DateTime.Now.AddHours(random.Next(4,10))),
                     random.Next(1,853)
                     )
    };

    entities.Flights.AddRange(flightsToSeed);
    entities.SaveChanges();

}



app.UseCors(builder => builder
.WithOrigins("*")
.AllowAnyMethod()
.AllowAnyHeader()
);
app.UseSwagger().UseSwaggerUI();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"
 );

app.MapFallbackToFile("index.html"); ;

app.Run();
