using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;


namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Add scoped = new instance per request
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(
                opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }
            );
            services.AddCors();
            services.AddAutoMapper(typeof(DatingRepository).Assembly);
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddDbContext<DataContext>(o =>
                o.UseSqlite(Configuration.GetConnectionString("defaultConnection"))); //appSettings.Dev.json
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IDatingRepository, DatingRepository>();
            services.AddScoped<LogUserActivity>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //--options to validate against our JWT auth
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            // services.AddSwaggerGen(c =>
            // {                                           // Name of teh API that we will be using
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            // });
        }

        // Use this method to configure the HTTP request pipeline.
        // This is middleware to interact w/ Req on its journey to deliver a Resp
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else    //Global ExceptionHandler..grabs exception and executes in alternate pipeline
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        //Set Response with this 500 status code
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        //store error and write/respond as/to response-body
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            //Extension method - AddApplicationError 
                            //deals w/ cors + Access-Control Headers + adds them to Resp
                            //Writes text that we output from program to the body
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            //app.UseHttpsRedirection(); //redirects to http's':XXXX when possible

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(req => req.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


            // app.UseSwaggerUI(c =>
            // {
            //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI");
            // });

            app.UseEndpoints(endpoints => // Endpoint API's rout incomming Req to Controllers
            {
                endpoints.MapControllers();  // Controllers map to the endpoints
            });
        }
    }
}

