using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class ClusterCoordinatorViewModel
    {
        [StringLength(61)]
        public string Name { get; set; }
        public int Id { get; set; }
    }
}