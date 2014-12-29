using System.ComponentModel;
namespace Models
{
    public class Disaster
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DisplayName("Is Currently Active?")]
        public bool IsActive { get; set; }
    }
}
