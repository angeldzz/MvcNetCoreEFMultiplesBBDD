using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using Oracle.ManagedDataAccess.Client;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    #region STORE PROCEDURES
    /*
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

--SI NECESITAMOS SELECT DENTRO DE UN PROCEDURE, DEBEMOS DEVOLVERLO
--COMO PARAMETRO DE SALIDA
CREATE OR REPLACE PROCEDURE SP_ALL_VEMPLEADOS
(p_cursor_empleados out SYS_REFCURSOR)
AS
BEGIN
    OPEN p_cursor_empleados FOR 
    SELECT * FROM V_EMPLEADOS;
END;    

CREATE OR REPLACE PROCEDURE SP_INSERT_EMPLEADO_DEPARTAMENTO
(p_apellido IN NVARCHAR2, p_oficio IN NVARCHAR2, p_dir IN NUMBER, p_salario IN NUMBER, 
p_comision IN NUMBER, p_deptnombre IN NVARCHAR2)
IS
	v_empNo NUMBER;
	v_deptno NUMBER;
BEGIN
	SELECT MAX(EMP_NO) + 1 INTO v_empNo FROM EMP;
	SELECT DEPT_NO INTO v_deptno FROM DEPT WHERE DEPT.DNOMBRE = p_deptnombre;

	INSERT INTO EMP (EMP_NO,APELLIDO,OFICIO,DIR,FECHA_ALT,SALARIO,COMISION,DEPT_NO) VALUES 
	(v_empNo, p_apellido, p_oficio, p_dir, SYSDATE, p_salario, p_comision, v_deptno);
END;
     */
    #endregion
    public class RepositoryEmpleadosOracle : IRepositoryEmpleados
    {
        private  HospitalContext context;
        public RepositoryEmpleadosOracle(HospitalContext context)
        {
            this.context = context;
        }
        public async Task<List<VistaEmpleado>> GetEmpleadosVistaAsync()
        {
            string sql = "begin ";
                sql += "SP_ALL_VEMPLEADOS (:p_cursor_empleados); ";
                sql += "end;";
            OracleParameter pamCursor = new OracleParameter();
            pamCursor.ParameterName = "p_cursor_empleados";
            pamCursor.Value = null;
            pamCursor.Direction = System.Data.ParameterDirection.Output;
            //INDICAMOS EL TIPO DE ORACLE
            pamCursor.OracleDbType = OracleDbType.RefCursor;
            var consulta = this.context.VistaEmpleados.FromSqlRaw(sql, pamCursor);
            return await consulta.ToListAsync();
        }
        public async Task<VistaEmpleado> GetDetallesEmpleadoAsync(int id)
        {
            var consulta = from datos in context.VistaEmpleados
                           where datos.Id_Empleado == id
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task InsertEmpleadoDepartamentoAsync(string apellido, string oficio, int dir, int salario, int comision, string NombreDept)
        {
            string sql = "BEGIN SP_INSERT_EMPLEADO_DEPARTAMENTO(:p_apellido, :p_oficio, :p_dir, :p_salario, :p_comision, :p_deptnombre); END;";

            OracleParameter pamapellido = new OracleParameter("p_apellido", apellido);
            OracleParameter pamoficio = new OracleParameter("p_oficio", oficio);
            OracleParameter pamdir = new OracleParameter("p_dir", dir);
            OracleParameter pamsalario = new OracleParameter("p_salario", salario);
            OracleParameter pamcomision = new OracleParameter("p_comision", comision);
            OracleParameter pamdeptnombre = new OracleParameter("p_deptnombre", NombreDept);

            await this.context.Database.ExecuteSqlRawAsync(sql, pamapellido, pamoficio, pamdir, pamsalario, pamcomision, pamdeptnombre);
        }
    }
}
