using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Models.Dto
{
    public class EmployeeDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

    }
}
