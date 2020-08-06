using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using kafkaAndDbPairing;
using kafkaAndDbPairing.Controllers;
using kafkaAndDbPairing.domain.repository;
using kafkaAndDbPairing.domain.service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace kafkaAndDbPairingTest.IntegrationTests
{
    public abstract class AbstractTestStartup
    {
        private readonly IConfiguration _configuration;
        protected AbstractTestStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkInMemoryDatabase();
            services.AddDbContext<DataContext>(builder => builder.UseInMemoryDatabase("TestDb"));
            services.AddMvc()
                .AddApplicationPart(typeof(OrdersController).Assembly)
                .AddMvcOptions(o =>
                {
                    o.EnableEndpointRouting = false;
                    o.InputFormatters.RemoveType<XmlDataContractSerializerInputFormatter>();
                    o.InputFormatters.RemoveType<XmlSerializerInputFormatter>();
                    o.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                    o.OutputFormatters.RemoveType<StreamOutputFormatter>();
                    o.OutputFormatters.RemoveType<StringOutputFormatter>();
                    o.OutputFormatters.RemoveType<XmlDataContractSerializerOutputFormatter>();
                    o.OutputFormatters.RemoveType<XmlSerializerOutputFormatter>();
                    o.Filters.Add(new AllowAnonymousFilter());
                });
            ConfigureServices2(services);
            AddExtraServices(services);
        }

        public void ConfigureServices2(IServiceCollection services)
        {
            /*services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));*/
            services.AddSingleton<IProducerService, ProducerService>();
            services.AddSingleton<IConsumerService, ConsumerService>();
            services.AddSingleton<IEmployeeProducerService, EmployeeProducerService>();
            services.AddSingleton<IEmployeeConsumerService, EmployeeConsumerService>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();

            services.AddControllers();
        }
        protected abstract void AddExtraServices(IServiceCollection services);
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
