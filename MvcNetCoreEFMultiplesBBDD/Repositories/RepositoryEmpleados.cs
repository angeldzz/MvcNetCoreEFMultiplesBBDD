using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    #region Views y Procedures
    /*
--VIEW DE SQL
CREATE PROCEDURE SP_INSERT_EMPLEADO_DEPARTAMENTO
(@apellido nvarchar(50), @oficio nvarchar(50), @dir int, @salario int, @comision int, @deptnombre nvarchar(50), @empNo int out)
AS
	select @empNo = MAX(EMP_NO) + 1 from EMP
	Declare @deptno int
	select @deptno = DEPT_NO FROM DEPT WHERE DEPT.DNOMBRE=@deptnombre

	INSERT INTO EMP (EMP_NO,APELLIDO,OFICIO,DIR,FECHA_ALT,SALARIO,COMISION,DEPT_NO) VALUES 
	(
	@empNo,@apellido,@oficio,@dir,GETDATE(),@salario,@comision,@deptno
	)
GO

-----------------------------------------------
exec SP_INSERT_EMPLEADO_DEPARTAMENTO 'ANGEL','CURRITO',3214,23000,12,'CONTABILIDAD'

     */
    #endregion
    public class RepositoryEmpleados: IRepositoryEmpleados
    {
        private HospitalContext context;
            public RepositoryEmpleados(HospitalContext context)
        {
            this.context = context;
        }
        public async Task<List<VistaEmpleado>> GetEmpleadosVistaAsync()
        {
            var consulta = from datos in context.VistaEmpleados
                           select datos;
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
            string sql = "SP_INSERT_EMPLEADO_DEPARTAMENTO @apellido,@oficio,@dir,@salario,@comision,@deptnombre, @deptno OUT";
            SqlParameter pamapellido = new SqlParameter("@apellido", apellido);
            SqlParameter pamoficio = new SqlParameter("@oficio", oficio);
            SqlParameter pamdir = new SqlParameter("@dir", dir);
            SqlParameter pamsalario = new SqlParameter("@salario", salario);
            SqlParameter pamcomision = new SqlParameter("@comision", comision);
            SqlParameter pamdeptnombre = new SqlParameter("@deptnombre", NombreDept);

            // 2. Definimos el parámetro de salida explícitamente
            SqlParameter pamEmpno = new SqlParameter
            {
                ParameterName = "@deptno",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output // Crucial
            };
            // 3. Ejecutamos y esperamos a que termine
            await context.Database.ExecuteSqlRawAsync(sql, pamapellido, pamoficio, pamdir, pamsalario, pamcomision, pamdeptnombre, pamEmpno);

            // 4. Recuperamos el valor después de la ejecución
            int idGenerado = (int)pamEmpno.Value;

            return idGenerado;
        }
    }
}
