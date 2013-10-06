using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrisisCheckinApp.ServiceClient
{
    public interface IDisasterService
    {
        SigninResponse Signin(SigninRequest request);

        GetDisastersResponse GetDisasters(GetDisastersRequest request);

        VolunteerResponse Volunteer(VolunteerRequest request);

        DisasterCheckinResponse DisasterCheckin(DisasterCheckinRequest request);
    }

    #region API Data Model

    public class Location
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }

    public class ServiceRequestBase
    {
        public string UserName { get; set; }
    }

    public class ServiceResponseBase
    {
        public string Error { get; set; }
    }

    public class SigninRequest : ServiceRequestBase
    {

    }

    public class SigninResponse : ServiceResponseBase
    {
    }

    public class Disaster
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Location Location { get; set; }
    }

    public class GetDisastersRequest : ServiceRequestBase
    {
        public Location Location { get; set; }
    }

    public class GetDisastersResponse : ServiceResponseBase
    {
        public IEnumerable<Disaster> Disasters { get; set; }
    }

    public class VolunteerRequest : ServiceRequestBase
    {
        public string DisasterId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class VolunteerResponse : ServiceResponseBase { }

    public class DisasterCheckinRequest : ServiceRequestBase
    {
        public Location Location { get; set; }
    }
    public class DisasterCheckinResponse : ServiceResponseBase { }


    #endregion 
}
