var builder = WebApplication.CreateBuilder(args);

builder.Authentication.AddJwtBearer();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
