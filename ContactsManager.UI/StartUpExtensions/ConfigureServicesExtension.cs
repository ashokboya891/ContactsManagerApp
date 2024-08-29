﻿using CRUDE.Filters.ActionFilters;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDE.StartUpExtensions
{
     //we have added configure services into  iservicecollection type so that we able to access in program.cs
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services,IConfiguration configuration)
        {

            services.AddTransient<ResponseHeaderActionFilter>();
            //it adds controller and views as services
            services.AddControllersWithViews(opt =>
            {
                //global filter added here 
                //opt.Filters.Add<ResponseHeaderActionFilter>(5);  //we can not pass params through this so prefer below line
                //or 
                //var logger= services.BuildServiceProvider().GetService<ILogger<ResponseHeaderActionFilter>>();
                //opt.Filters.Add(new ResponseHeaderActionFilter(logger,"key-from-global","value-from-global",2));
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
                // opt.Filters.Add(new ResponseHeaderActionFilter("key-from-global", "value-from-global", 2));  //filter attribute class
                opt.Filters.Add(new ResponseHeaderActionFilter(logger)
                {
                    Key = "mykey-from-global",
                    Value = "myvalue-from-global",
                    Order = 3
                });
            });


            services.AddDbContext<ApplicationDbContext>(opt => {
                opt.UseSqlServer(configuration.GetConnectionString("con"));
            });

            services.AddScoped<IPersonRespository, PersonRepository>();
            services.AddScoped<ICountrysRepository, CountriesRepository>();


            //add services into IOC container
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IPersonGetterService, PersonGetterService>();

            //below 3 lines is for implementing OCP & OCP with interface and inheritence
            services.AddScoped<IPersonGetterService, PersonsGetterServiceWithFewExcelFields>();  //ocp with interface
           // services.AddScoped<IPersonGetterService, PersonGetterServiceChild>();   //ocp with inheritence
            services.AddScoped<PersonGetterService, PersonGetterService>();


            services.AddScoped<IPersonAdderService, PersonAdderService>();
            services.AddScoped<IPersonDeleterService, PersonDeleterService>();
            services.AddScoped<IPersonUpdaterServices, PersonUpdaterService>();
            services.AddScoped<IPersonSorterService, PersonSorterService>();


            services.AddTransient<PersonsListActionFilter>();
            //services.AddScoped<ICountryService, CountryService>();

            services.AddHttpLogging(opt =>
            {
                opt.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });
            return services;
        }
    }
}
