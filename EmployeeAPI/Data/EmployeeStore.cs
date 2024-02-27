using EmployeeAPI.Models.Dto;

namespace EmployeeAPI.Data
{
    public static class EmployeeStore
    {
        public static List<EmployeeDTO> employeeList = new List<EmployeeDTO>
            {
                new EmployeeDTO { Id = 1, Name = "Steve Nash", Age = 21, Job = "Developer", Country = "USA"},
                new EmployeeDTO { Id = 2, Name = "Itachi Uchiha", Age = 31, Job = "Tiktokerist", Country = "Japan" },
                new EmployeeDTO { Id = 3, Name = "Juan Tamad", Age = 18, Job = "Engineer", Country = "Philippines" },
            };
    }
}
