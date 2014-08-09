using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CrisisCheckin.Shared
{
	#region Request and Response objects
	public class RegisterRequest : ServiceRequest
	{
		public override string ServiceMethodUrl { get { return "register"; } }

		public string FirstName { get;set; }
		public string LastName { get;set; }
		public string Email { get;set; }
		public string Username { get;set; }
		public string Token { get;set; }
		public int ClusterId { get;set; }
	}

	public class RegisterResponse : ServiceResponse<bool>
	{
	}

	public class SignInRequest : ServiceRequest
	{
		public override string ServiceMethodUrl { get {	return "signin"; } }

		public string Username { get;set; }
		public string Password { get;set; }
	}
	public class SignInResponse : ServiceResponse<bool>
	{
	}

	public class SignOutRequest : ServiceRequest
	{
		public override string ServiceMethodUrl { get {	return "signout"; } }

		public string Username { get;set; }
	}
	public class SignOutResponse : ServiceResponse<bool>
	{
	}

	public class ClustersRequest : ServiceRequest 
	{
		public override string ServiceMethodUrl { get {	return "clusters"; } }
	}
	public class ClustersResponse : ServiceResponse<IEnumerable<Cluster>>
	{
	}

	public class CheckInRequest : ServiceRequest
	{
		public override string ServiceMethodUrl { get {	return "checkin"; } }

		public string Username { get;set; }
		public string Password { get;set; }

		public int DisasterId { get;set; }
		public double Latitude { get;set; }
		public double Longitude { get;set; }
	}
	public class CheckInResponse : ServiceResponse<bool>
	{
	}

	public class CheckOutRequest : ServiceRequest
	{
		public override string ServiceMethodUrl { get {	return "checkout"; } }

		public string Username { get;set; }
		public string Password { get;set; }
	}
	public class CheckOutResponse : ServiceResponse<bool>
	{
	}

	public class DisastersRequest : ServiceRequest
	{
		public override string ServiceMethodUrl { get {	return "disasters"; } }

		public double Latitude { get;set; }
		public double Longitude { get;set; }
	}
	public class DisastersResponse : ServiceResponse<IEnumerable<Disaster>>
	{
	}

	public class VolunteerRequest : ServiceRequest
	{
		public override string ServiceMethodUrl { get {	return "volunteer"; } }

		public string Username { get;set; }
		public string Password { get;set; }

		public int DisasterId { get;set; }
		public DateTime StartDate { get;set; }
		public DateTime EndDate { get;set; }
	}
	public class VolunteerResponse : ServiceResponse<bool>
	{
	}

	public class CommitmentsRequest : ServiceRequest 
	{
		public override string ServiceMethodUrl { get {	return "commitments"; } }

		public string Username { get;set; }
		public string Password { get;set; }
	}
	public class CommitmentsResponse : ServiceResponse<IEnumerable<Commitment>>
	{
	}

	public abstract class ServiceRequest
	{
		public abstract string ServiceMethodUrl { get; }
	}

	public abstract class ServiceResponse<TResult>
	{
		public bool Succeeded { get; set; }
		public Exception Exception { get;set; }
		public TResult Result { get;set; }
	}
	#endregion

	#region Models
	public class Disaster
	{
		public int Id { get;set; }
		public string Name { get;set; }
		public bool IsActive { get;set; }
		public string Location { get;set; }
	}

	public class Cluster 
	{
		public int Id { get;set; }
		public string Name { get;set; }
	}

	public class Commitment
	{
		public int DisasterId { get;set; }
		public DateTime StartDate { get;set; }
		public DateTime EndDate { get;set; }
		public bool IsActive { get;set; }

		public Disaster Disaster { get;set; }
	}
	#endregion

	#region WebService Definition and Implementation
	public class WebServiceFactory
	{
		public static IWebService Create()
		{
			return new MockWebService ();
		}
	}

	public interface IWebService
	{
		Task<RegisterResponse> RegisterAsync (RegisterRequest request);
		Task<SignInResponse> SignInAsync (SignInRequest request);
		Task<SignOutResponse> SignOutAsync (SignOutRequest request);
		Task<ClustersResponse> GetClustersAsync (ClustersRequest request);
		Task<CheckInResponse> CheckInAsync (CheckInRequest request);
		Task<CheckOutResponse> CheckOutAsync (CheckOutRequest request);
		Task<VolunteerResponse> VolunteerAsync (VolunteerRequest request);
		Task<DisastersResponse> GetDisastersAsync (DisastersRequest request);
		Task<CommitmentsResponse> GetCommitmentsAsync (CommitmentsRequest request);
	}

	public class WebService : IWebService
    {
		const string API_URL_BASE = "http://crisischeckin.azurewebsites.net/api/";

		#region IWebService implementation

		public async Task<RegisterResponse> RegisterAsync (RegisterRequest request)
		{
			throw new NotImplementedException ();
		}

		public async Task<SignInResponse> SignInAsync (SignInRequest request)
		{
			throw new NotImplementedException ();
		}

		public async Task<SignOutResponse> SignOutAsync (SignOutRequest request)
		{
			throw new NotImplementedException ();
		}

		public async Task<ClustersResponse> GetClustersAsync (ClustersRequest request)
		{
			throw new NotImplementedException ();
		}

		public async Task<CheckInResponse> CheckInAsync (CheckInRequest request)
		{
			throw new NotImplementedException ();
		}

		public async Task<CheckOutResponse> CheckOutAsync (CheckOutRequest request)
		{
			throw new NotImplementedException ();
		}

		public async Task<VolunteerResponse> VolunteerAsync (VolunteerRequest request)
		{
			throw new NotImplementedException ();
		}

		public async Task<DisastersResponse> GetDisastersAsync (DisastersRequest request)
		{
			throw new NotImplementedException ();
		}

		public async Task<CommitmentsResponse> GetCommitmentsAsync (CommitmentsRequest request)
		{

			throw new NotImplementedException ();
		}

		#endregion

		async Task<string> Request(string method, params KeyValuePair<string, string>[] parameters)
		{
			return string.Empty;
		}
    }
	#endregion
}
