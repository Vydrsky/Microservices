using SharedKernel.Abstractions.Behaviors;
using SharedKernel.Abstractions.Behaviours;
using SharedKernel.Utility.Exceptions.Handlers;
using SharedKernel.Utility.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//DI
var assembly = Assembly.GetExecutingAssembly();

builder.Services.AddCarterWithAssemblies(assembly);

builder.Services.AddMediatR(config => {
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(config => {
    config.Connection(builder.Configuration.GetConnectionString("Database") ?? "");
}).UseLightweightSessions();

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

//Middleware Pipeline
app.MapCarter();

app.UseExceptionHandler(config => { });

await app.RunAsync();
