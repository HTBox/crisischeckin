using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Disaster
    {
        public int Id { get; set; }
        [StringLength(50), ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
