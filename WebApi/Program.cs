using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using WebApi.Contexts;
using WebApi.Helpers.DataSeeding;
using WebApi.Middleware.ExceptionHandler;
using WebApi.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<AppSettings>();
builder.Services.AddDbContext<DbContext, ApplicationDbContext>();
builder.Services.AddHelpers(builder.Environment);
builder.Services.AddServices();
if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddBackgroundJobs();
}

JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
{
    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
};

builder.Services.AddControllers()
.AddNewtonsoftJson(o =>
{
    o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddApiVersioning(o =>
{
    o.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(o =>
{
    o.GroupNameFormat = "'v'VVV";
    o.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,

            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };
        c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "Bearer",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });

        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "User Api",
            Description = "An api to manage user",
        });

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    }
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = "*",
         ValidAudience = "*",
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SkewWombatChickenPlusAP"))
     };
 });
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "User v1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "User v2");
});
}


app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader());

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();

if (!app.Environment.IsEnvironment("Test"))
{
    await ApplicationSeedData.EnsureDataAsync(app.Services);
}


app.MapControllers();

app.Run();

public partial class Program { }