using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class Request
    {
        [Required]
        public string Name { get; set; }
    }
}