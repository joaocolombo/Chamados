using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Data.Repositories;
using FakeRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;

namespace API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();


        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddOptions();
            //  services.Configure<Chamado>(Configuration);
            services.AddCors(o => o.AddPolicy("LiberarAcessoExterno", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddMvc();
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));
            });
            services.AddScoped<IChamadoService, ChamadoService>();
            services.AddScoped<IChamadoRepository, ChamadoRepository>();
            services.AddScoped<IEventoRepository, EventoRepository>();
            services.AddScoped<IEventoService, EventoService>();
            services.AddScoped<IFilialService, FilialService>();
            services.AddScoped<IFilialRepository, FilialRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<ICategoriaService, CategoriaServise>();
            services.AddScoped<IFilaService, FilaService>();
            services.AddScoped<IFilaRepository, FilaRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {


            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseCors("LiberarAcessoExterno");

            app.UseMvc();


        }
    }
}
