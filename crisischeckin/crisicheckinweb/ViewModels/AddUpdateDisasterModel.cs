using Models;

namespace crisicheckinweb.ViewModels
{

    public enum EditMode { Creating, Updating }
    
    public class AddUpdateDisasterModel
    {
        public Disaster Disaster { get; set; }

        public EditMode EditMode { get; set; }
    }
}