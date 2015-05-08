using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class UnassignClusterCoordinatorViewModel
    {
        public int DisasterId { get; set; }
        public int CoordinatorId { get; set; }

        [Required]
        [StringLength(61)]//30 first name + space + 30 last name
        [Display(Name = "Coordinator")]
        public string CoordinatorName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Cluster")]
        public string ClusterName { get; set; }
    }
}