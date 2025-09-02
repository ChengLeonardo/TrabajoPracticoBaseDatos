namespace Trivago.Core.Ubicacion;

public class TipoHabitacion
{
    public uint idTipo { get; set; }
    public string Nombre { get; set; }
    public List<Habitacion> Habitaciones { get; set; }
}