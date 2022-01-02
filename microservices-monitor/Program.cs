using microservices_monitor.BackgoundServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace microservices_monitor // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader());
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<ServiceStatusCollector>();
            builder.Services.AddSingleton<ServicesRepository>();
            


            // doing stuf 

            var serviceProvider = builder.Services.BuildServiceProvider();

            var serviceStatusCollector = ActivatorUtilities.CreateInstance<ServiceStatusCollector>(serviceProvider);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseCors();

            app.Run();
        }

        
    }
}