

using Trivago.Core.Ubicacion;

namespace DTOs;

public record struct PaisDTO(uint idPais, string Nombre);
public record struct PaisDetalleDTO(uint idPais, string Nombre, List<CiudadDTO> CiudadDTOs);
public record struct PaisAltaDTO(string Nombre);
public record struct CiudadDTO(uint idCiudad, string Nombre);

public record struct CiudadDetalleDTO(uint idCiudad, string Nombre, List<HotelDTO> HotelDTOs);

public record struct CiudadAltaDTO(uint idPais, string Nombre);
public record struct HotelDTO(uint idHotel, string Nombre, string Direccion, string Telefono, string URL);

public record struct HabitacionDTO(uint idHabitacion, decimal PrecioPorNoche);
public record struct HabitacionDetalleDTO(uint idHabitacion, decimal PrecioPorNoche, List<ComentarioDTO> ComentarioDTOs, List<ReservaDTO> ReservaDTOs);
public record struct HabitacionAltaDTO(uint idHotel, uint idTipo, decimal PrecioPorNoche);
public record struct ComentarioDTO(uint idComentario, DateTime Fecha, string comentario, sbyte Calificacion);
public record struct ReservaDTO(uint idReserva, DateTime Entrada, DateTime Salida, decimal Precio, uint Telefono);