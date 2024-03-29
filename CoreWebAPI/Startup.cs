﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using CoreDAL.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace CoreWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddMvcOptions(options =>
                {
                    options.ReturnHttpNotAcceptable = true;
                })
                .AddXmlSerializerFormatters();
            services.AddDbContext<OrionDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OrionDBConnection")));
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                    .WithOrigins("*")
                    .WithMethods("*")
                    .WithHeaders("*")
                    ;

                });
                options.AddPolicy("AnotherPolicy", builder =>
                {
                    builder
                    .WithOrigins("*")
                    .WithMethods("*")
                    .WithHeaders("*")
                    ;
                });
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("V1", new Info { Title = "EmployeeService", Version = "V1" });
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/V1/swagger.json", "EmployeeServiceV1.0");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
