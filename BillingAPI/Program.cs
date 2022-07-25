using BillingAPI.Data;
using BillingAPI.Extensions;
using BillingAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("jwt").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddApplicationServices(builder.Configuration);

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LogRequestMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

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