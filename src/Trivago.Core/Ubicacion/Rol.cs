namespace Trivago.Core.Ubicacion;

public class Rol
{
    public uint idRol { get; set; }  
    public string Nombre {get; set;}
    public List<Usuario> Usuarios {get; set;}
}