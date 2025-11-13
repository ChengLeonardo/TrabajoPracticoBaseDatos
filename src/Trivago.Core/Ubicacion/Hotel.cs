namespace Trivago.Core.Ubicacion;

public class Hotel
{
    public uint idHotel { get; set; }
    public uint idCiudad { get; set; }
    public Ciudad ciudad { get; set; }
    public List<Habitacion>? Habitaciones { get; set; }
    public string Nombre { get; set; }
    public string Direccion  { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string URL { get; set; } 
}