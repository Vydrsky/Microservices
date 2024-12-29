var builder = WebApplication.CreateBuilder(args);

//DI


var app = builder.Build();

//Middleware Pipeline


await app.RunAsync();
