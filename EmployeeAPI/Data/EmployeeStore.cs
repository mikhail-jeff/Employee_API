using EmployeeAPI.Models.Dto;

namespace EmployeeAPI.Data
{
    public static class EmployeeStore
    {
        public static List<EmployeeDTO> employeeList = new List<EmployeeDTO>
            {
                new EmployeeDTO { Id = 1, Name = "John Doe"},
                new EmployeeDTO { Id = 2, Name = "Steve Nash" },
            };
    }
}
