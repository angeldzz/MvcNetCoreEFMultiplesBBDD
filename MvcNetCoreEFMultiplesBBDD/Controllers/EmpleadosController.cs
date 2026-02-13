using Microsoft.AspNetCore.Mvc;
using MvcNetCoreEFMultiplesBBDD.Models;
using MvcNetCoreEFMultiplesBBDD.Repositories;

namespace MvcNetCoreEFMultiplesBBDD.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;
        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<EmpleadoDepartamento> empleados = await this.repo.GetEmpleadosDepartamentoAsync();
            return View(empleados);
        }
        public async Task<IActionResult> Details(int id)
        {
            EmpleadoDepartamento empleado = await this.repo.FindEmpleadoAsync(id);
            return View(empleado);
        }
    }
}
