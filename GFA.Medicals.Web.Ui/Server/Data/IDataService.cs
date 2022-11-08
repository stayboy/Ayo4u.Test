using GFA.Medicals.Web.Ui.Shared.Models.Employee;
using GFA.Medicals.Web.Ui.Shared.Models.Sales;
using System.Collections.Generic;

namespace GFA.Medicals.Web.Ui.Server.Data
{
    public interface IDataService
    {
        List<Employee> GetEmployees();
        IEnumerable<Sale> GetSales();
        List<Team> GetTeams();
    }
}
