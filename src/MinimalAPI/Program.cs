using System.Data;
using MySqlConnector;
using Scalar.AspNetCore;
using Trivago.RepoDapper;
using Trivago.Core;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MySQL");

builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));

builder.Services.AddScoped<IRepoPaisAsync, RepoPaisAsync>();
builder.Services.AddScoped<IRepoCiudadAsync, RepoCiudadAsync>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

app.MapGet("/pais", async (IRepoPaisAsync repo) =>
    await repo.ListarAsync()
        is List<Pais> paises
            ? Results.Ok(paises)
            : Results.NotFound());

app.MapGet("/pais/{id}", async (uint id, IRepoPaisAsync repo) =>
    await repo.DetalleAsync(id)
        is Pais pais
            ? Results.Ok(pais)
            : Results.NotFound());

app.MapPost("/pais", async (string nombre, IRepoPaisAsync repo) =>
{
    Pais pais = new Pais{
        Nombre = nombre
    };
    await repo.AltaAsync(pais);

    return Results.Created($"/paisitems/{pais.idPais}", pais);
});


app.MapGet("/ciudad", async (IRepoCiudadAsync repo) =>
    await repo.ListarAsync()
        is List<Ciudad> ciudades
            ? Results.Ok(ciudades)
            : Results.NotFound());

app.MapGet("/ciudad/{id}", async (uint id, IRepoCiudadAsync repo) =>
    await repo.DetalleAsync(id)
        is Ciudad ciudad
            ? Results.Ok(ciudad)
            : Results.NotFound());

app.MapPost("/ciudad", async (string nombre, uint idPais, IRepoCiudadAsync repo) =>
{
    Ciudad ciudad = new Ciudad{
        idPais = idPais,
        nombre = nombre
    };
    await repo.AltaAsync(ciudad);

    return Results.Created($"/ciudaditems/{ciudad.idPais}", ciudad);
});

app.Run();
