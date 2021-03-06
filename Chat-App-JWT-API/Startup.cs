using Chat_App_Bussiness_Logic.Services;
using Chat_App_Bussiness_Logic.Configuration;
using Chat_App_JWT_API.Middleware;
using Chat_App_Library.Interfaces;
using Chat_App_Library.Singletons;
using Chat_App_Logic.Mocks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_App_JWT_API
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
            DatabaseSingleton.GetSingleton().SetRepository(new MockingRepository());
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Chat_App_JWT_API", Version = "v1" });
            });
            services.AddCors(options => {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

            var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false
            };
            services.AddSingleton(tokenValidationParameters);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParameters;
            });

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).addEntityFrameworkStores
            //timestamp 28:00

            services.AddSingleton(typeof(IDatabaseSingleton), DatabaseSingleton.GetSingleton());
            services.AddSingleton(typeof(IChatService),new ChatService(DatabaseSingleton.GetSingleton()));
            services.AddSingleton(typeof(IGroupService), new GroupService(DatabaseSingleton.GetSingleton()));
            services.AddSingleton(typeof(IInvitationService), new InvitationService(DatabaseSingleton.GetSingleton()));
            services.AddSingleton<ICredentialsService, CredentialsService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat_App_JWT_API v1"));
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
