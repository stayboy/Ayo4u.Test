using GFA.Medicals.Server.Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddCookie().AddJwtBearer();

var app = builder.Build();

app.MapGroup("coding").MapConversionsApi(app.Logger);

app.MapGet("/", () => "Hello World!");

app.Run();
