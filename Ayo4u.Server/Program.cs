var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddApiFramework(app.Configuration);

app.UseSharedInfrastructure();

app.MapUsersApi();
app.MapGroup("/conversions").MapConversionsApi().RequireAuthorization();

app.Run();
