using System.ComponentModel.DataAnnotations;
namespace Models
{
    public class Cluster
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

    }
}
