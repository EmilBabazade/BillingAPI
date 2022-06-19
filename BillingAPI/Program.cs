using BillingAPI.Data;
using BillingAPI.Extensions;
using BillingAPI.Middlewares;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(builder.Configuration);

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await SeedDatabase();

app.Run();

async Task SeedDatabase()
{
    using IServiceScope scope = app.Services.CreateScope();
    IServiceProvider services = scope.ServiceProvider;
    try
    {
        DataContext dataContext = services.GetRequiredService<DataContext>();
        await dataContext.Database.MigrateAsync();
        await Seed.SeedData(dataContext);
    }
    catch (Exception ex)
    {
        ILogger logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error ocurred during migration");
    }
}
