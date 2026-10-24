namespace Academic360.Application.Common.Interfaces;

public interface IUsuarioActual
{
    long ObtenerIdUsuario();

    // Método para limitar consultas por área 
    int ObtenerIdArea();

    // Método para validar permisos específicos dentro de la lógica de negocio 
    bool PerteneceARol(string rolEsperado);
}