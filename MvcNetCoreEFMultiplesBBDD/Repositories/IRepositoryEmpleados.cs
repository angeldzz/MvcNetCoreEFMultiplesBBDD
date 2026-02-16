using MvcNetCoreEFMultiplesBBDD.Models;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public interface IRepositoryEmpleados
    {
        Task<List<VistaEmpleado>> GetEmpleadosVistaAsync();
        Task<VistaEmpleado> GetDetallesEmpleadoAsync(int id);
        Task InsertEmpleadoDepartamentoAsync(string apellido, string oficio, int dir, int salario, int comision, string NombreDept);
    }
}
