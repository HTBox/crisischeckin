using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CrisisCheckin.Shared
{
	public class MockWebService : IWebService
	{
		public MockWebService ()
		{
		}

		#region IWebService implementation

		public async Task<RegisterResponse> RegisterAsync (RegisterRequest request)
		{
			await Task.Delay (2000);

			return new RegisterResponse {
				Succeeded = true,
				Result = true
			};
		}

		public async Task<SignInResponse> SignInAsync (SignInRequest request)
		{
			await Task.Delay (2000);

			return new SignInResponse {
				Succeeded = true,
				Result = true
			};
		}

		public async Task<SignOutResponse> SignOutAsync (SignOutRequest request)
		{
			await Task.Delay (2000);

			return new SignOutResponse {
				Succeeded = true,
				Result = true
			};
		}

		public async Task<ClustersResponse> GetClustersAsync (ClustersRequest request)
		{
			await Task.Delay (2000);

			return new ClustersResponse {
				Succeeded = true,
				Result = new List<Cluster> {
					new Cluster { Name = "Agriculture Cluster", Id = 1 },
					new Cluster { Name = "Camp Coordination and Management Cluster", Id = 2 },
					new Cluster { Name = "Early Recovery Cluster", Id = 3 },
					new Cluster { Name = "Emergency Shelter Cluster", Id = 4 },
					new Cluster { Name = "Emergency Telecommunications Cluster", Id = 5 },
					new Cluster { Name = "Food Cluster", Id = 6 },
					new Cluster { Name = "Health Cluster", Id = 7 },
					new Cluster { Name = "Logistics Cluster", Id = 8 },
					new Cluster { Name = "Nutrition Cluster", Id = 9 },
					new Cluster { Name = "Protection Cluster", Id = 10 },
					new Cluster { Name = "Water and Sanitation Cluster", Id = 11 },
				}
			};
		}

		public async Task<CheckInResponse> CheckInAsync (CheckInRequest request)
		{
			await Task.Delay (2000);

			return new CheckInResponse {
				Succeeded = true,
				Result = true
			};
		}

		public async Task<CheckOutResponse> CheckOutAsync (CheckOutRequest request)
		{
			await Task.Delay (2000);

			return new CheckOutResponse {
				Succeeded = true,
				Result = true
			};
		}

		public async Task<VolunteerResponse> VolunteerAsync (VolunteerRequest request)
		{
			await Task.Delay (2000);

			return new VolunteerResponse {
				Succeeded = true,
				Result = true
			};
		}

		public async Task<DisastersResponse> GetDisastersAsync (DisastersRequest request)
		{
			await Task.Delay (2000);

			return new DisastersResponse {
				Succeeded = true,
				Result = new List<Disaster> {
					new Disaster {
						Id = 1,
						IsActive = true,
						Name = "Earthquake",
						Location = "Panguna, Papa New Guinea"
					},
					new Disaster {
						Id = 2,
						IsActive = true,
						Name = "Mudslide",
						Location = "Oso, Washington"
					},
					new Disaster {
						Id = 3,
						IsActive = false,
						Name = "Tsunami",
						Location = "Ao Nang, Thailand"
					},
					new Disaster {
						Id = 4,
						IsActive = false,
						Name = "Hurricane",
						Location = "Haiti"
					}
				}
			};
		}

		public async Task<CommitmentsResponse> GetCommitmentsAsync (CommitmentsRequest request)
		{
			await Task.Delay (2000);

			return new CommitmentsResponse {
				Succeeded = true,
				Result = new List<Commitment> {
					new Commitment {
						DisasterId = 2,
						IsActive = true,
						StartDate = DateTime.UtcNow.AddDays(-1),
						EndDate = DateTime.UtcNow.AddDays(5)
					},
					new Commitment {
						DisasterId = 3,
						IsActive = false,
						StartDate = DateTime.UtcNow.AddYears(-1),
						EndDate = DateTime.UtcNow.AddYears(-1).AddDays(10)
					},
					new Commitment {
						DisasterId = 4,
						IsActive = false,
						StartDate = DateTime.UtcNow.AddYears(-1).AddDays(180),
						EndDate = DateTime.UtcNow.AddYears(-1).AddDays(190)
					}
				}
			};
		}

		#endregion
	}
}

