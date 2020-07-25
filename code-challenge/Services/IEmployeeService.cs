using challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Services
{
    public interface IEmployeeService
    {
        Employee GetById(String id);
        Employee Create(Employee employee);
        Compensation CreateCompensation(Compensation compensation);
        Employee Replace(Employee originalEmployee, Employee newEmployee);
        ReportingStructure GetReportsById(String id);
        Compensation GetCompensationByEmployeeId(string id);
    }
}
