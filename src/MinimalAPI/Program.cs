using System.Data;
using MySqlConnector;
using Scalar.AspNetCore;
using Trivago.RepoDapper;
using Trivago.Core;
using Trivago.Core.Persistencia;
using Trivago.Core.Ubicacion;
using DTOs;

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
            ? Results.Ok(paises.Select(pais => new PaisDTO(pais.idPais, pais.Nombre)))
            : Results.NotFound());

app.MapGet("/pais/{id}", async (uint id, IRepoPaisAsync repo) =>
    await repo.DetalleAsync(id)
        is Pais pais
            ? Results.Ok(new PaisDetalleDTO(pais.idPais, pais.Nombre, pais.Ciudades.Select(ciudad => new CiudadDTO(ciudad.idCiudad, ciudad.nombre)).ToList()))
            : Results.NotFound());

app.MapPost("/pais", async (PaisAltaDTO pais, IRepoPaisAsync repo) =>
{
    Pais paisAlta = new Pais{
        Nombre = pais.Nombre
    };
    await repo.AltaAsync(paisAlta);

    return Results.Created($"/paisitems/{paisAlta.idPais}", pais);
});


app.MapGet("/ciudad", async (IRepoCiudadAsync repo) =>
    await repo.ListarAsync()
        is List<Ciudad> ciudades
            ? Results.Ok(ciudades.Select(ciudad => new CiudadDTO(ciudad.idCiudad, ciudad.nombre)))
            : Results.NotFound());

app.MapGet("/ciudad/{id}", async (uint id, IRepoCiudadAsync repo) =>
    await repo.DetalleAsync(id)
        is Ciudad ciudad
            ? Results.Ok(new CiudadDetalleDTO(ciudad.idCiudad, ciudad.nombre, ciudad.Hoteles.Select(hotel => new HotelDTO(hotel.idHotel, hotel.Nombre, hotel.Direccion, hotel.Telefono, hotel.URL)).ToList()))
            : Results.NotFound());

app.MapPost("/ciudad", async (CiudadAltaDTO ciudad, IRepoCiudadAsync repo) =>
{
    Ciudad ciudadAlta = new Ciudad{
        idPais = ciudad.idPais,
        nombre = ciudad.Nombre
    };
    await repo.AltaAsync(ciudadAlta);

    return Results.Created($"/ciudaditems/{ciudadAlta.idCiudad}", ciudad);
});

app.Run();
