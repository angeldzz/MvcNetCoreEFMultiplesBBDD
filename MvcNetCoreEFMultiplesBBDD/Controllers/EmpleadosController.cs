using Microsoft.AspNetCore.Mvc;
using MvcNetCoreEFMultiplesBBDD.Models;
using MvcNetCoreEFMultiplesBBDD.Repositories;

namespace MvcNetCoreEFMultiplesBBDD.Controllers
{
    public class EmpleadosController : Controller
    {
        private IRepositoryEmpleados repo;
        public EmpleadosController(IRepositoryEmpleados repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<VistaEmpleado> empleados = await this.repo.GetEmpleadosVistaAsync();
            return View(empleados);
        }
        public async Task<IActionResult> Details(int id)
        {
            VistaEmpleado empleado = await this.repo.GetDetallesEmpleadoAsync(id);
            return View(empleado);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string apellido, string oficio, int dir, int salario, int comision,
            string NombreDept)
        {
            int empNo = await this.repo.InsertEmpleadoDepartamentoAsync(apellido, oficio, dir, salario, comision, NombreDept);
            return RedirectToAction("Details", new { id = empNo });
        }
    }
}
