using challenge.Models;
using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace challenge.Controllers
{
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpPost("{id}/compensation", Name = "addCompensation")]
        public IActionResult CreateCompensation(String id, [FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for employee '{id} with {compensation.Salary} on {compensation.EffectiveDate} '");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            compensation.Employee = employee;
            var returnedCompensation = _employeeService.CreateCompensation(compensation);

            return CreatedAtRoute("getCompensationById", new CompensationViewModel { CompensationId = returnedCompensation.CompensationId, EffectiveDate = returnedCompensation.EffectiveDate, EmployeeId = returnedCompensation.Employee.EmployeeId, Salary = returnedCompensation.Salary });
        }

        [HttpGet("{id}/compensation", Name = "getCompensationById")]
        public IActionResult GetCompensationByEmployeeId(String id)
        {
            _logger.LogDebug($"Received compensation get request for '{id}'");
            var compensation = _employeeService.GetCompensationByEmployeeId(id);
            if (compensation == null)
                return NotFound();

            return Ok(new CompensationViewModel { CompensationId = compensation.CompensationId, EffectiveDate = compensation.EffectiveDate, Salary = compensation.Salary, EmployeeId = compensation.Employee.EmployeeId });
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpGet("{id}/reports", Name = "getEmployeeReports")]
        public IActionResult GetEmployeeReports(String id)
        {
            _logger.LogDebug($"Received get reports request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            var reports = _employeeService.GetReportsById(id);

            return Ok(reports);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody] Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }
    }
}
