using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Disaster
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
