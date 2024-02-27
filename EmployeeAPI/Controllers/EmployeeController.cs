using EmployeeAPI.Data;
using EmployeeAPI.Models;
using EmployeeAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        // dependency injection
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }


        // GET ALL EMPLOYEES
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeDTO>> GetEmployees()
        {
            _logger.LogInformation("Getting All Employees");
            return Ok(EmployeeStore.employeeList);
        }

        // GET A SINGLE EMPLOYEE
        [HttpGet("{id:int}", Name = "GetEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EmployeeDTO> GetEmployee(int id)
        {

            if (id == 0)
            {
                _logger.LogError($"Get employee error with an id: {id}");
                return BadRequest();
            }

            var employee = EmployeeStore.employeeList.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // CREATE EMPLOYEE
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<EmployeeDTO> CreateEmployee([FromBody] EmployeeDTO employeeDTO)
        {
            // Custom Validation
            if (EmployeeStore.employeeList.FirstOrDefault(e => e.Name.ToLower() == employeeDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Employee already exists!");

                return BadRequest(ModelState);
            }

            if (employeeDTO == null)
            {
                return BadRequest(employeeDTO);
            }

            if (employeeDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            employeeDTO.Id = EmployeeStore.employeeList.OrderByDescending(e => e.Id).FirstOrDefault().Id + 1;

            EmployeeStore.employeeList.Add(employeeDTO);

            return CreatedAtRoute("GetEmployee", new { id = employeeDTO.Id }, employeeDTO);
        }


        // DELETE EMPLOYEE
        [HttpDelete("{id:int}", Name = "DeleteEmployee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteEmployee(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var employee = EmployeeStore.employeeList.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            EmployeeStore.employeeList.Remove(employee);

            return NoContent();
        }

        // UPDATE EMPLOYEE
        [HttpPut("{id:int}", Name = "UpdateEmployee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateEmployee(int id, [FromBody] EmployeeDTO employeeDTO)
        {
            if (employeeDTO == null || id != employeeDTO.Id)
            {
                return BadRequest();
            }

            var employee = EmployeeStore.employeeList.FirstOrDefault(e => e.Id == id);

            employee.Name = employeeDTO.Name;
            employee.Age = employeeDTO.Age;
            employee.Job = employeeDTO.Job;
            employee.Country = employeeDTO.Country;

            return NoContent();
        }


        // UPDATE EMPLOYEE
        [HttpPatch("{id:int}", Name = "UpdatePartialEmployee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialEmployee(int id, JsonPatchDocument<EmployeeDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var employee = EmployeeStore.employeeList.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            patchDTO.ApplyTo(employee, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

    }
}
