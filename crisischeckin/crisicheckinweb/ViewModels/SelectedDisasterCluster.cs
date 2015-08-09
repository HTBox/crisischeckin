using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class SelectedDisasterCluster
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public bool Selected { get; set; }
    }
}