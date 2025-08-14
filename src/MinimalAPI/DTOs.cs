

using Trivago.Core.Ubicacion;

namespace DTOs;

public record struct PaisDTO(uint idPais, string Nombre);
public record struct PaisDetalleDTO(uint idPais, string Nombre, List<CiudadDTO> CiudadDTOs);
public record struct PaisAltaDTO(string Nombre);
public record struct CiudadDTO(uint idCiudad, string Nombre);

public record struct CiudadDetalleDTO(uint idCiudad, string Nombre, List<HotelDTO> HotelDTOs);

public record struct CiudadAltaDTO(uint idPais, string Nombre);
public record struct HotelDTO(uint idHotel, string Nombre, string Direccion, string Telefono, string URL);