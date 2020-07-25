using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        Employee GetByIdWithReports(String id);
        Employee Add(Employee employee);
        Compensation AddCompensation(Compensation compensation);
        Employee Remove(Employee employee);
        Task SaveAsync();
        Compensation GetCompensationByEmployeeId(string id);
    }
}