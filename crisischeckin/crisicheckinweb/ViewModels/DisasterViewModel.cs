using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class DisasterViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Disaster name")]
        public string Name { get; set; }

        [DisplayName("Currently active")]
        public bool IsActive { get; set; }
    }
}