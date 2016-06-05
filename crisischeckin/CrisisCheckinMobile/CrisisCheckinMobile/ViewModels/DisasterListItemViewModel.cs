
namespace CrisisCheckinMobile.ViewModels
{
    public class DisasterListItemViewModel
    {
        private readonly string _disasterName;
        private readonly string _disasterStatusAndDate;
        private readonly int _id;

        public DisasterListItemViewModel(int id, string disasterName, 
            string disasterStatusAndDate, CommitmentViewModel commitmentData)
        {
            _disasterName = disasterName;
            _disasterStatusAndDate = disasterStatusAndDate;
            _id = id;
            CommitmentData = commitmentData;
        }

        public CommitmentViewModel CommitmentData { get; set; }

        public int Id
        {
            get { return _id; }
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