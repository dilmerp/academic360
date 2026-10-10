namespace Academic360.Domain.Common;

public abstract class EntidadAuditable
{
    public long UsuarioCreacion { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public long? UsuarioModificacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
}