using System.ComponentModel.DataAnnotations;
namespace Models
{
    public class ClusterGroup
    {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

    }
}
