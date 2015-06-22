
namespace CrisisCheckinMobile
{
    public class DisasterListDto
    {
        private readonly string _disasterName;
        private readonly string _disasterStatusAndDate;

        public DisasterListDto(string disasterName, string disasterStatusAndDate)
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

