using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Compensation AddCompensation(Compensation compensation)
        {
            _employeeContext.Employees.FirstOrDefault(e => e.EmployeeId == compensation.Employee.EmployeeId).Compensation = compensation;
            return compensation;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees.Include(e => e.Compensation).SingleOrDefault(e => e.EmployeeId == id);
        }

        public Compensation GetCompensationByEmployeeId(string id)
        {
            return _employeeContext.Employees.Include(e => e.Compensation).FirstOrDefault(e => e.EmployeeId == id).Compensation;
        }

        public Employee GetByIdWithReports(string id)
        {
            return _employeeContext.Employees
                .Include(i => i.DirectReports)
                .ThenInclude(r => r.DirectReports)
                .SingleOrDefault(e => e.EmployeeId == id);
        }
      
        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

    }
}
