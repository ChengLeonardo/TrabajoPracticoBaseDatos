namespace Trivago.Core.Ubicacion;
public class Pais
{
    public int idPais { get; set; }
    public List<Ciudad> Ciudades { get; set; }  
    public string Nombre { get; set; }
}