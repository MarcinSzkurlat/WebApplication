using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using WebApplicationAPI;
using WebApplicationAPI.Extensions;
using WebApplicationAPI.Interfaces.Services;
using WebApplicationAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiExtensions(builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Logger(lc =>
    {
        lc.Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Information);
        lc.WriteTo.File("Logs/Info/info_.txt", rollingInterval: RollingInterval.Day);
    })
    .WriteTo.Logger(lc =>
    {
        lc.MinimumLevel.Warning();
        lc.WriteTo.File("Logs/Errors/error_.txt", rollingInterval: RollingInterval.Day);
    })
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    bool dataBaseHasAnyRecord = false;

    try
    {
        using var scope = app.Services.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetService<WebApplicationDbContext>();
        await dbContext.Database.MigrateAsync();

        dataBaseHasAnyRecord = dbContext.Tags.Any();
    }
    catch (Exception exception)
    {
        Log.Fatal($"Error while creating database | {exception}");
    }

    if (!dataBaseHasAnyRecord)
    {
        try
        {
            int.TryParse(app.Configuration["PagesToFetch"], out int pagesToFetch);

            using var scope = app.Services.CreateScope();
            var stackOverflowApiService = scope.ServiceProvider.GetService<IStackOverflowApiService>();
            await stackOverflowApiService.GetTagsAsync(pagesToFetch);
        }
        catch (Exception exception)
        {
            Log.Fatal($"Error while fetching data | {exception}");
        }
    }
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
