using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    #region Views y Procedures
    /*
--VIEW DE SQL
create view V_EMPLEADOS
as

SELECT EMP_NO, APELLIDO, SALARIO,DEPT.DEPT_NO, DEPT.DNOMBRE, DEPT.LOC FROM EMP
INNER JOIN DEPT
ON DEPT.DEPT_NO = EMP.DEPT_NO
go
    -----------------------------------------------
    --View de ORACLE

CREATE OR REPLACE VIEW V_EMPLEADOS AS
SELECT 
    EMP.EMP_NO,
    EMP.APELLIDO,
    EMP.SALARIO,
    DEPT.DEPT_NO,
    DEPT.DNOMBRE,
    DEPT.LOC
FROM 
    EMP
INNER JOIN 
    DEPT ON DEPT.DEPT_NO = EMP.DEPT_NO;

     */
    #endregion
    public class RepositoryEmpleados
    {
        private EmpleadosContext context;
            public RepositoryEmpleados(EmpleadosContext context)
        {
            this.context = context;
        }
        public async Task<List<EmpleadoDepartamento>> GetEmpleadosDepartamentoAsync()
        {
            var consulta = from datos in context.Empleados
                           select datos;
            return await consulta.ToListAsync();
        }
        public async Task<EmpleadoDepartamento> FindEmpleadoAsync(int idEmpleado)
        {
            var consulta = from datos in context.Empleados
                           where datos.Id_Empleado == idEmpleado
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }
    }
}
