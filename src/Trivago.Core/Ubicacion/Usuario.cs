namespace Trivago.Core.Ubicacion;

public class Usuario
{
    public uint idUsuario { get; set; }
    public string Nombre { get; set; }  
    public string Apellido { get; set; }
    public string Mail { get; set; }
    public string Contrasena { get; set; }
    public uint idRol {get;set;}
    public Rol Rol {get;set;}
    public List<Reserva> Reservas { get; set; }
}