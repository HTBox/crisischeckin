using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
POST  /signup?fname=limor&lname=lahiani&phone=&email=limorl@Microsoft.com&uname=limonit
&token=xyz&clusterId=123

POST /signin?uname=limonit

POST /signout?uname=limonit

GET /clusters

GET /disasters?lat=73.15141&lon=45.9393939393

POST /volunteer?uname=limonit&did=4353535&sDate=848484&eDate=949494

POST /checkin?uname=limonit&did=55353&lat=56.777777&lon=75.95959595  

POST /checkout?uname=limonit
 */

namespace CrisisCheckinApp.ServiceClient
{
    public class DisasterServiceClient : IDisasterService
    {
        public SigninResponse Signin(SigninRequest request)
        {
            return new SigninResponse();
        }

        public GetDisastersResponse GetDisasters(GetDisastersRequest request)
        {
            return new GetDisastersResponse
            {
                Disasters = new List<Disaster>{
                    new Disaster{ Name = "Tzunami in Thailan", Id = Guid.NewGuid().ToString(), Location = new Location { Latitude = 73.6262626, Longitude = 64.4449949 }, IsActive = true },
                    new Disaster{ Name = "Hurican in Miami", Id = Guid.NewGuid().ToString(), Location = new Location { Latitude = 73.6262626, Longitude = 76.696969 }, IsActive = true },
                    new Disaster{ Name = "Tornado in Cuba", Id = Guid.NewGuid().ToString(), Location = new Location { Latitude = 73.6262626, Longitude = 32.4449949 }, IsActive = true }    
                }
            };
        }

        public VolunteerResponse Volunteer(VolunteerRequest request)
        {
            return new VolunteerResponse();
        }

        public DisasterCheckinResponse DisasterCheckin(DisasterCheckinRequest request)
        {
            return new DisasterCheckinResponse();
        }

    }
}


