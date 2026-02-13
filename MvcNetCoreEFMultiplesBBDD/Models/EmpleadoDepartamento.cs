using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcNetCoreEFMultiplesBBDD.Models
{
    [Table ("V_EMPLEADOS")]
    public class EmpleadoDepartamento
    {
        [Key]
        [Column("EMP_NO")]
        public int Id_Empleado { get; set; }
        [Column("APELLIDO")]
        public string Apellido { get; set; }
        [Column("SALARIO")]
        public int Salario { get; set; }
        [Column("DEPT_NO")]
        public int Dept_NO { get; set; }
        [Column("DNOMBRE")]
        public string DeptNombre { get; set; }
        [Column("LOC")]
        public string Localidad { get; set; }

    }
}
