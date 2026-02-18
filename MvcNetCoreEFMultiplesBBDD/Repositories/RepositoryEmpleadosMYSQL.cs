using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using MySql.Data.MySqlClient;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    #region SP
    /*
use hospital;
CREATE OR REPLACE VIEW V_EMPLEADOS
AS
SELECT EMP.EMP_NO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO, DEPT.DEPT_NO,DEPT.DNOMBRE,DEPT.LOC FROM EMP
INNER JOIN DEPT ON EMP.DEPT_NO = DEPT.DEPT_NO;

SELECT * FROM v_empleados

DELIMITER //

CREATE PROCEDURE SP_ALL_VEMPLEADOS()
BEGIN
    -- En MySQL no necesitas parámetros de salida para devolver tablas
    SELECT * FROM V_EMPLEADOS;
END //

DELIMITER ;
------------------------------------------------
    
DELIMITER //

CREATE PROCEDURE SP_INSERT_EMPLEADO_DEPARTAMENTO
(
    IN p_apellido VARCHAR(50), 
    IN p_oficio VARCHAR(50), 
    IN p_dir INT, 
    IN p_salario INT, 
    IN p_comision INT, 
    IN p_deptnombre VARCHAR(50)
)
BEGIN
    DECLARE v_empNo INT;
    DECLARE v_deptno INT;

    SELECT IFNULL(MAX(EMP_NO), 0) + 1 INTO v_empNo FROM EMP;

    SELECT DEPT_NO INTO v_deptno 
    FROM DEPT 
    WHERE DNOMBRE = p_deptnombre 
    LIMIT 1;

    INSERT INTO EMP (EMP_NO, APELLIDO, OFICIO, DIR, FECHA_ALT, SALARIO, COMISION, DEPT_NO) 
    VALUES (v_empNo, p_apellido, p_oficio, p_dir, NOW(), p_salario, p_comision, v_deptno);
END //

DELIMITER ;
     */
    #endregion
    public class RepositoryEmpleadosMYSQL : IRepositoryEmpleados
    {
        private HospitalContext context;
        public RepositoryEmpleadosMYSQL(HospitalContext context)
        {
            this.context = context;
        }
        
        //public async Task<List<VistaEmpleado>> GetEmpleadosVistaAsync()
        //{
        //    var consulta = from datos in context.Empleados
        //                   select datos;
        //    return await consulta.ToListAsync();
        //}
        public async Task<List<VistaEmpleado>> GetEmpleadosVistaAsync()
        {
            // 1. En MySQL la sintaxis para ejecutar un procedimiento es "CALL"
            // No necesitas bloques "begin...end" ni declarar parámetros para el cursor
            string sql = "CALL SP_ALL_VEMPLEADOS()";

            // 2. En MySQL no existe el tipo RefCursor. 
            // El procedimiento simplemente ejecuta un SELECT y EF Core captura el resultado.
            // Si el procedimiento no tiene parámetros de entrada, no necesitas crear MySqlParameters.

            var consulta = this.context.VistaEmpleados.FromSqlRaw(sql);

            return await consulta.ToListAsync();
        }
        public async Task<VistaEmpleado> GetDetallesEmpleadoAsync(int id)
        {
            var consulta = from datos in context.VistaEmpleados
                           where datos.Id_Empleado == id
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<int> InsertEmpleadoDepartamentoAsync(string apellido, string oficio, int dir, int salario, int comision, string NombreDept)
        {
            string sql = "CALL SP_INSERT_EMPLEADO_DEPARTAMENTO(@p_apellido, @p_oficio, @p_dir, @p_salario, @p_comision, @p_deptnombre)";
            
            var p_apellido = new MySqlParameter("@p_apellido", apellido);
            var p_oficio = new MySqlParameter("@p_oficio", oficio);
            var p_dir = new MySqlParameter("@p_dir", dir);
            var p_salario = new MySqlParameter("@p_salario", salario);
            var p_comision = new MySqlParameter("@p_comision", comision);
            var p_deptnombre = new MySqlParameter("@p_deptnombre", NombreDept);

            await this.context.Database.ExecuteSqlRawAsync(sql,
                p_apellido, p_oficio, p_dir, p_salario, p_comision, p_deptnombre);
            // En MySQL, el procedimiento no devuelve un valor de salida.
            return 1;
        }
    }
}
