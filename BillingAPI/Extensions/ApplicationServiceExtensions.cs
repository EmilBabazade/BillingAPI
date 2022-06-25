﻿using BillingAPI.Data;
using BillingAPI.Helpers;
using BillingAPI.Services.jwt;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddMediatR(typeof(Program));
            services.AddScoped<IJWTService, JWTService>();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
