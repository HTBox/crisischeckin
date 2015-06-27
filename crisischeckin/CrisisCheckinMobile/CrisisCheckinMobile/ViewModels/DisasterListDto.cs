
namespace CrisisCheckinMobile
{
    public class DisasterListViewModel
    {
        private readonly string _disasterName;
        private readonly string _disasterStatusAndDate;

        public DisasterListViewModel(string disasterName, string disasterStatusAndDate)
        {
            _disasterName = disasterName;
            _disasterStatusAndDate = disasterStatusAndDate;
        }

        public string DisasterName
        {
            get
            {
                return _disasterName;
            }
        }

        public string DisasterStatusAndDate
        {
            get
            {
                return _disasterStatusAndDate;
            }
        }
    }
}

