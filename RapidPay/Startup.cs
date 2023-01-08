using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RapidPay.Data.DbContexts;
using RapidPay.Data.Models;
using RapidPay.Data.Repositories;
using RapidPay.Fees.Services.UEF;
using RapidPay.Fees.Services.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllers();
            services.AddDbContext<RapidPayDbContext>
                (o => o.UseInMemoryDatabase("RapidPayDatabase"));

            services.AddScoped<ICardRepository, CardRepository>();
            services.AddSingleton<IUFEService, UFEService>();
            services.AddScoped<IFeeCalculatorService, FeeCalculatorService>();

            #region Authorization/Authentication

            var issuer = Configuration.GetSection("Jwt").GetSection("Issuer").Value;
            var audience = Configuration.GetSection("Jwt").GetSection("Audience").Value;
            var secretKey = Configuration.GetSection("Jwt").GetSection("SecretKey").Value;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(JwtPolicies.Admin, JwtPolicies.AdminPolicy());
                config.AddPolicy(JwtPolicies.User, JwtPolicies.UserPolicy());
            });

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("RapidPay APIs.");
                });
            });

        }
    }
}
