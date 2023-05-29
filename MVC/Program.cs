using Microsoft.AspNetCore.Identity;
using MVC.Data;
using MVC.Managers;
using MVC.Settings;
using Persistence;
using Persistence.Entities;
using Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<AppSettings>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddHttpClients();

builder.Services.AddDefaultIdentity<UserEntity>()
.AddRoles<IdentityRole<Guid>>()
.AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserManager<FinalUserManager>()
    .AddSignInManager<FinalSignInManager>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

await ApplicationSeedData.EnsureData(app.Services);

app.Run();
