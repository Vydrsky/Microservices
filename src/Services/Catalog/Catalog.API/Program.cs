using SharedKernel.Utility.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//DI
var assembly = Assembly.GetExecutingAssembly();

builder.Services.AddCarterWithAssemblies(assembly);

builder.Services.AddMediatR(config => {
    config.RegisterServicesFromAssembly(assembly);
});

builder.Services.AddMarten(config => {
    config.Connection(builder.Configuration.GetConnectionString("Database") ?? "");
}).UseLightweightSessions();

var app = builder.Build();

//Middleware Pipeline
app.MapCarter();

await app.RunAsync();
