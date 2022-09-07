using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NotepadBasedCalculator.Shared;

namespace NotepadBasedCalculator.WebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services
                .AddApiVersioning(
                    config =>
                    {
                        // reporting api versions will return the headers
                        // "api-supported-versions" and "api-deprecated-versions"
                        config.ReportApiVersions = true;
                    })
                .AddApiExplorer(
                    setup =>
                    {
                        setup.GroupNameFormat = "'v'VVV";
                        setup.SubstituteApiVersionInUrl = true;
                    })
                .AddMvc();

            builder.Services
                .AddAuthentication(
                    opt =>
                    {
                        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            RequireExpirationTime = true,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,
                            LifetimeValidator = LifetimeValidator,
                            ValidIssuer = ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                            ValidAudience = ConfigurationManager.AppSetting["JWT:ValidAudience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]))
                        };

                        static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
                        {
                            if (expires != null)
                            {
                                return expires > DateTime.UtcNow;
                            }
                            return false;
                        }
                    });

            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
            builder.Services.AddSingleton(typeof(IConfigurationReader), new ConfigurationReader());

            WebApplication app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); // TODO: should we remove this?

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();
                    foreach (ApiVersionDescription description in descriptions)
                    {
                        string url = $"/swagger/{description.GroupName}/swagger.json";
                        string name = description.GroupName.ToUpperInvariant();
                        options.SwaggerEndpoint(url, name);
                    }
                });
            }

            app.Run();
        }
    }
}
