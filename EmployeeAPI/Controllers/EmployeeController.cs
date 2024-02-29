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
        private readonly ApplicationDbContext _db;

        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }


        // GET ALL EMPLOYEES
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeDTO>> GetEmployees()
        {
            return Ok(_db.Employees);
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

                return BadRequest();
            }

            var employee = _db.Employees.FirstOrDefault(e => e.Id == id);

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
            if (_db.Employees.FirstOrDefault(e => e.Name.ToLower() == employeeDTO.Name.ToLower()) != null)
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

            Employee model = new()
            {
                Name = employeeDTO.Name,
                Job = employeeDTO.Job,
                Country = employeeDTO.Country,
                CreatedDate = DateTime.Now
            };

            _db.Employees.Add(model);
            _db.SaveChanges();

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

            var employee = _db.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            _db.Employees.Remove(employee);
            _db.SaveChanges();

            return NoContent();
        }

        // UPDATE EMPLOYEE
        [HttpPut("{id:int}", Name = "UpdateEmployee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateEmployee(int id, [FromBody] EmployeeDTO employeeDTO)
        {
            if (employeeDTO == null)
            {
                return BadRequest("Employee data is missing.");
            }

            if (id != employeeDTO.Id)
            {
                // Log the mismatched IDs for debugging
                Console.WriteLine($"URL ID: {id}, DTO ID: {employeeDTO.Id}");

                // Fix the ID mismatch error
                employeeDTO.Id = id;

            }

            var existingEmployee = _db.Employees.FirstOrDefault(e => e.Id == id);

            if (existingEmployee == null)
            {
                return NotFound("Employee not found.");
            }

            try
            {
                existingEmployee.Name = employeeDTO.Name;
                existingEmployee.Job = employeeDTO.Job;
                existingEmployee.Country = employeeDTO.Country;

                _db.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error updating employee: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the employee.");
            }
        }


        // PARTIAL UPDATE EMPLOYEE
        [HttpPatch("{id:int}", Name = "UpdatePartialEmployee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialEmployee(int id, JsonPatchDocument<EmployeeDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var employee = _db.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            // Store the existing CreatedDate
            var createdDate = employee.CreatedDate;

            EmployeeDTO employeeDTO = new EmployeeDTO()
            {
                Name = employee.Name,
                Job = employee.Job,
                Country = employee.Country,
            };

            patchDTO.ApplyTo(employeeDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Apply changes to the employee entity
            employee.Name = employeeDTO.Name;
            employee.Job = employeeDTO.Job;
            employee.Country = employeeDTO.Country;

            // Restore the CreatedDate
            employee.CreatedDate = createdDate;

            _db.SaveChanges();

            return NoContent();
        }


    }
}
