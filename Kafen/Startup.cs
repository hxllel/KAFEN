using Kafen.Datos;
using Kafen.Datos.Repositorios;
using Kafen.Transversal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Kafen
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var options = new DbContextOptionsBuilder<ContextoKafen>()
                .UseSqlServer(Configuration
                .GetConnectionString("DefaultConnection"))
                .Options;
            ContextoKafen contexto = new ContextoKafen(options);
            contexto.Database.EnsureCreated();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<IServicioEmail, ServicioEmail>();
            services.AddScoped<IRepositorioArticulo, RepositorioArticulo>();
            services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
            services.AddScoped<IRepositorioCarrito, RepositorioCarrito>();
            services.AddScoped<IRepositorioEstatus, RepositorioEstatus>();
            services.AddScoped<IRepositorioPedidos, RepositorioPedidos>();
            services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            services.AddScoped<IRepositorioDetalleVenta, RepositorioDetalleVenta>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Home";
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    option.AccessDeniedPath = "/Home";

                }
                );





            services.AddDbContext<ContextoKafen>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
