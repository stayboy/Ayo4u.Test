using GFA.Medicals.Web.Ui.Server.Data;
using GFA.Medicals.Web.Ui.Shared.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFA.Medicals.Web.Ui.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IDataService _dataService;
        public EmployeeController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var result = _dataService.GetEmployees().ToList();
            return result;
        }
    }
}
