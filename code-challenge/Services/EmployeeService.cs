using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public Compensation GetCompensationByEmployeeId(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return  _employeeRepository.GetCompensationByEmployeeId(id);
            }

            return null;
        }

        public ReportingStructure GetReportsById(string id)
        {
            var reportingStructure = new ReportingStructure();
            if (!String.IsNullOrEmpty(id))
            {
                var employee =  _employeeRepository.GetByIdWithReports(id);
                reportingStructure.Employee = employee;

                var reports = employee.DirectReports;

                while (reports.Count != 0)
                {
                   reportingStructure.NumberOfReports += reports.Count;
                   reports = GetAllReports(reports);
                }
            }
            return reportingStructure;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        public Compensation CreateCompensation(Compensation compensation)
        {
            if (compensation.Employee != null)
            {
                _employeeRepository.AddCompensation(compensation);
                _employeeRepository.SaveAsync().Wait();
            }

            return new Compensation
            { 
                EffectiveDate = compensation.EffectiveDate, 
                Salary = compensation.Salary, 
                Employee = new Employee 
                { 
                    EmployeeId = compensation.Employee.EmployeeId, 
                    Department = compensation.Employee.Department, 
                    DirectReports = compensation.Employee.DirectReports, 
                    FirstName = compensation.Employee.FirstName, 
                    LastName = compensation.Employee.LastName, 
                    Position = compensation.Employee.Position
                } 
            };
        }

        private List<Employee> GetAllReports(List<Employee> employees)
        {
            var employeeReports = new List<Employee>();
            foreach (var employee in employees)
            {
                if (employee.DirectReports != null)
                    employeeReports.AddRange(employee.DirectReports);
            }

            return employeeReports;
        }
    }
}
