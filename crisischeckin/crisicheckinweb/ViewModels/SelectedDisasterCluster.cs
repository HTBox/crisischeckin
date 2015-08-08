using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class SelectedDisasterCluster
    {
        [StringLength(50)]
        public string Name { get; set; }

        public bool Selected { get; set; }
    }
}