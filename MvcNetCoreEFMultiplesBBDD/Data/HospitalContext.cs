using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Models;

namespace MvcNetCoreEFMultiplesBBDD.Data
{

    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
        {}
        public DbSet<VistaEmpleado> VistaEmpleados { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
    }
}
