using Coding.Challenge.API.Extensions;
using Coding.Challenge.Dependencies.Database;
using Coding.Challenge.Dependencies.Database.Interfaces;
using Coding.Challenge.Dependencies.Services;

var builder = WebApplication.CreateBuilder(args)
        .ConfigureWebHost();
        
builder.RegisterServices(builder.Configuration);
builder.Services.AddSingleton<ILoggerService, LoggerService>();
var app = builder.Build();

app.MapControllers();
app.UseSwagger()
    .UseSwaggerUI();

app.Run();