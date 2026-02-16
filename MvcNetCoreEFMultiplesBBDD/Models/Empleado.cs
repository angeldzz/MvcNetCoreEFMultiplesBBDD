using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcNetCoreEFMultiplesBBDD.Models
{
    [Table("EMP")]
    public class Empleado
    {
        [Key]
        [Column("EMP_NO")]
        public int Id_Empleado { get; set; }
        [Column("APELLIDO")]
        public string Apellido { get; set; }
        [Column("OFICIO")]
        public string Oficio { get; set; }
        [Column("FECHA_ALT")]
        public DateTime FechaAlt { get; set; }
        [Column("DIR")]
        public int Dir { get; set; }
        [Column("SALARIO")]
        public int Salario { get; set; }
        [Column("COMISION")]
        public int Comision { get; set; }
        [Column("DEPT_NO")]
        public int DeptNo { get; set; }
    }
}
