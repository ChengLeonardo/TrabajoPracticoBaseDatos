using System.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MySqlConnector;
using Trivago.Core;
using Trivago.Core.Persistencia;
using Trivago.RepoDapper;
var builder = WebApplication.CreateBuilder(args);



var connectionString = builder.Configuration.GetConnectionString("MySQL");

builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRepoPaisAsync, RepoPaisAsync>();
builder.Services.AddScoped<IRepoCiudadAsync, RepoCiudadAsync>();
builder.Services.AddScoped<IRepoHotelAsync, RepoHotelAsync>();
builder.Services.AddScoped<IRepoHabitacionAsync, RepoHabitacionAsync>();
builder.Services.AddScoped<IRepoReservaAsync, RepoReservaAsync>();
builder.Services.AddScoped<IRepoUsuarioAsync, RepoUsuarioAsync>();
builder.Services.AddScoped<IRepoComentarioAsync, RepoComentarioAsync>();
builder.Services.AddScoped<IRepoMetodoPagoAsync, RepoMetodoPagoAsync>();
builder.Services.AddScoped<IRepoReservaAsync, RepoReservaAsync>();
builder.Services.AddScoped<IRepoTipoHabitacionAsync, RepoTipoHabitacionAsync>();
builder.Services.AddScoped<IRepoRolAsync, RepoRolAsync>();
builder.Services.AddAuthentication(options =>
{
    // Set the default scheme to use
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    // Configure cookie settings like login path, etc.
    options.LoginPath = "/Usuario/Login"; 
    options.LogoutPath = "/Usuario/Logout";
    options.AccessDeniedPath = "/Home/AccessDenied";
});

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
