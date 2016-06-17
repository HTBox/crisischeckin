using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Models;
using Moq;
using NUnit.Framework;
using Services.Exceptions;
using Services.Interfaces;

namespace Services.UnitTest
{
    [TestFixture]
    public class RequestServiceTest
    {
        private Request _unassignedRequest;
        private Request _assignedRequest;
        private Request _completedRequest;
        // for the sorting tests - method expects IEnumerable
        private IEnumerable<Request> _requestList;
        // for the async filtering tests - method expects IQueryable 
        // Must implement IDbAsyncQueryProvider
        private Mock<IQueryable<Request>> _mockSet;

        private Mock<IDataService> _mockDataService;
        private RequestService _requestService;

        [SetUp]
        public void CreateDependencies()
        {
            _assignedRequest = new Request()
            {
                RequestId = 1,
                CreatedDate = DateTime.Now.AddDays(-1).Date,
                EndDate = DateTime.Today.AddDays(10).Date,
                Description = "Assigned Request",
                CreatorId = 1,
                AssigneeId = 2,
                Completed = false,
                Location = "B"
            };
            _unassignedRequest = new Request()
            {
                RequestId = 2,
                CreatedDate = DateTime.Now.AddDays(-2).Date,
                EndDate = DateTime.Today.AddDays(20).Date,
                Description = "Unassigned Request",
                CreatorId = 1,
                Completed = false,
                Location = "A"
            };
            _completedRequest = new Request()
            {
                RequestId = 3,
                CreatedDate = DateTime.Now.AddDays(-3).Date,
                EndDate = DateTime.Today.AddDays(30).Date,
                Description = "Completed Request",
                CreatorId = 1,
                AssigneeId = 2,
                Completed = true,
                Location = "C"
            };

            var data = new List<Request>
            {
                _assignedRequest,
                _unassignedRequest,
                _completedRequest
            }.AsQueryable();

            _requestList = data;

            // prepare mock queryable set the implements DbAsyncQueryProvider
            _mockSet = new Mock<IQueryable<Request>>();
            _mockSet.As<IDbAsyncEnumerable<Request>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Request>(data.GetEnumerator()));

            _mockSet.As<IQueryable<Request>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Request>(data.Provider));

            _mockSet.As<IQueryable<Request>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<Request>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<Request>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            // Set up the data service
            _mockDataService = new Mock<IDataService>();
            _mockDataService.Setup(c => c.Requests).Returns(_mockSet.Object);

            // set up request service
            _requestService = new RequestService(_mockDataService.Object);
        }

        [Test]
        public async Task Get_Assigned_NonCompleted_Requests_ForTestUser()
        {
            var requests = await _requestService.GetRequestForUserAsync(2);

            Assert.AreEqual(1, requests.Count());
            Assert.AreEqual(_assignedRequest, requests.First());
        }

        [Test]
        public async Task Get_NonAssigned_NonCompleted_Requests()
        {
            var requests = await _requestService.GetOpenRequestsAsync();

            Assert.AreEqual(1, requests.Count());
            Assert.AreEqual(_unassignedRequest, requests.First());
        }

        [Test]
        public async Task FilterRequest_ByNothing()
        {
            var results = await _requestService.FilterRequestsAsync(new RequestSearch(), _mockSet.Object);

            Assert.AreEqual(results.Count(), 3);
        }

        [Test]
        public async Task FilterRequest_ByDescription()
        {
            var results =
                await
                    _requestService.FilterRequestsAsync(new RequestSearch() {Description = "Assigned Request"},
                        _mockSet.Object);

            Assert.AreEqual(results.Count(), 1);
            Assert.AreEqual(results.First(), _assignedRequest);
        }

        [Test]
        public async Task FilterRequest_ByDescription_ReturnMultiple()
        {
            var results =
                await
                    _requestService.FilterRequestsAsync(new RequestSearch() {Description = "Request"}, _mockSet.Object);

            Assert.AreEqual(results.Count(), 3);
        }

        [Test]
        public async Task FilterRequest_ByLocation()
        {
            var results =
                await _requestService.FilterRequestsAsync(new RequestSearch() {Location = "C"}, _mockSet.Object);

            Assert.AreEqual(results.Count(), 1);
            Assert.AreEqual(results.First(), _completedRequest);
        }

        [Test]
        public async Task FilterRequest_ByNullableCreatedDate()
        {
            var results =
                await
                    _requestService.FilterRequestsAsync(
                        new RequestSearch() {NullableCreatedDate = DateTime.Now.AddDays(-1).Date}, _mockSet.Object);

            Assert.AreEqual(results.Count(), 1);
            Assert.AreEqual(results.First(), _assignedRequest);
        }

        [Test]
        public async Task FilterRequest_ByNullableEndDate()
        {
            var results =
                await
                    _requestService.FilterRequestsAsync(
                        new RequestSearch() {NullableEndDate = DateTime.Today.AddDays(30).Date}, _mockSet.Object);

            Assert.AreEqual(results.Count(), 1);
            Assert.AreEqual(results.First(), _completedRequest);
        }

        [Test]
        public async Task FilterRequest_ByRequestStatus_All()
        {
            var results =
                await
                    _requestService.FilterRequestsAsync(new RequestSearch() {RequestStatus = RequestStatus.All},
                        _mockSet.Object);

            Assert.AreEqual(results.Count(), 3);
        }

        [Test]
        public async Task FilterRequest_ByRequestStatus_Assigned()
        {
            var results =
                await
                    _requestService.FilterRequestsAsync(new RequestSearch() {RequestStatus = RequestStatus.Assigned},
                        _mockSet.Object);

            Assert.AreEqual(results.Count(), 1);
            Assert.AreEqual(results.First(), _assignedRequest);
        }

        [Test]
        public async Task FilterRequest_ByRequestStatus_Unassigned()
        {
            var results =
                await
                    _requestService.FilterRequestsAsync(new RequestSearch() {RequestStatus = RequestStatus.Unassigned},
                        _mockSet.Object);

            Assert.AreEqual(results.Count(), 1);
            Assert.AreEqual(results.First(), _unassignedRequest);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public async Task FilterRequest_ByInvalidRequestStatus()
        {
            await
                _requestService.FilterRequestsAsync(new RequestSearch() {RequestStatus = (RequestStatus) 5},
                    _mockSet.Object);
        }


        [Test]
        public async Task FilterRequest_ByRequestStatus_Completed()
        {
            var results =
                await
                    _requestService.FilterRequestsAsync(new RequestSearch() {RequestStatus = RequestStatus.Completed},
                        _mockSet.Object);

            Assert.AreEqual(results.Count(), 1);
            Assert.AreEqual(results.First(), _completedRequest);
        }


        [Test]
        public void SortRequests_Descending_ByCreatedDate()
        {
            var displayName = GetDisplayName("CreatedDate");

            var sortedList = _requestService.SortRequests(displayName, "desc", _requestList);

            Assert.AreSame(sortedList.ElementAt(0), _assignedRequest);
            Assert.AreSame(sortedList.ElementAt(1), _unassignedRequest);
            Assert.AreSame(sortedList.ElementAt(2), _completedRequest);
        }

        [Test]
        public void SortRequests_Ascending_ByCreatedDate()
        {
            var displayName = GetDisplayName("CreatedDate");
            var sortedList = _requestService.SortRequests(displayName, "asc", _requestList);

            Assert.AreSame(sortedList.ElementAt(2), _assignedRequest);
            Assert.AreSame(sortedList.ElementAt(1), _unassignedRequest);
            Assert.AreSame(sortedList.ElementAt(0), _completedRequest);
        }

        [Test]
        public void SortRequests_Descending_ByEndDate()
        {
            var displayName = GetDisplayName("EndDate");

            var sortedList = _requestService.SortRequests(displayName, "desc", _requestList);

            Assert.AreSame(sortedList.ElementAt(2), _assignedRequest);
            Assert.AreSame(sortedList.ElementAt(1), _unassignedRequest);
            Assert.AreSame(sortedList.ElementAt(0), _completedRequest);
        }

        [Test]
        public void SortRequests_Ascending_ByEndDate()
        {
            var displayName = GetDisplayName("EndDate");

            var sortedList = _requestService.SortRequests(displayName, "asc", _requestList);

            Assert.AreSame(sortedList.ElementAt(0), _assignedRequest);
            Assert.AreSame(sortedList.ElementAt(1), _unassignedRequest);
            Assert.AreSame(sortedList.ElementAt(2), _completedRequest);
        }

        [Test]
        public void SortRequests_Descending_ByLocation()
        {
            var displayName = GetDisplayName("Location");

            var sortedList = _requestService.SortRequests(displayName, "desc", _requestList);

            Assert.AreSame(sortedList.ElementAt(1), _assignedRequest);
            Assert.AreSame(sortedList.ElementAt(2), _unassignedRequest);
            Assert.AreSame(sortedList.ElementAt(0), _completedRequest);
        }

        [Test]
        public void SortRequests_Ascending_ByLocation()
        {
            var displayName = GetDisplayName("Location");

            var sortedList = _requestService.SortRequests(displayName, "asc", _requestList);

            Assert.AreSame(sortedList.ElementAt(1), _assignedRequest);
            Assert.AreSame(sortedList.ElementAt(0), _unassignedRequest);
            Assert.AreSame(sortedList.ElementAt(2), _completedRequest);
        }

        [Test]
        public void SortRequests_Descending_ByDescription()
        {
            var displayName = GetDisplayName("Description");

            var sortedList = _requestService.SortRequests(displayName, "desc", _requestList);

            Assert.AreSame(sortedList.ElementAt(2), _assignedRequest);
            Assert.AreSame(sortedList.ElementAt(0), _unassignedRequest);
            Assert.AreSame(sortedList.ElementAt(1), _completedRequest);
        }

        [Test]
        public void SortRequests_Ascending_ByDescription()
        {
            var displayName = GetDisplayName("Description");

            var sortedList = _requestService.SortRequests(displayName, "asc", _requestList);

            Assert.AreSame(sortedList.ElementAt(0), _assignedRequest);
            Assert.AreSame(sortedList.ElementAt(2), _unassignedRequest);
            Assert.AreSame(sortedList.ElementAt(1), _completedRequest);
        }

        [Test]
        public void SortRequests_Descending_ByCompleted()
        {
            var displayName = GetDisplayName("Completed");

            var sortedList = _requestService.SortRequests(displayName, "desc", _requestList);

            Assert.AreSame(sortedList.ElementAt(1), _assignedRequest);
            Assert.AreSame(sortedList.ElementAt(2), _unassignedRequest);
            Assert.AreSame(sortedList.ElementAt(0), _completedRequest);
        }

        [Test]
        public void SortRequests_Ascending_ByCompleted()
        {
            var displayName = GetDisplayName("Completed");

            var sortedList = _requestService.SortRequests(displayName, "asc", _requestList);

            Assert.AreSame(sortedList.ElementAt(0), _assignedRequest);
            Assert.AreSame(sortedList.ElementAt(1), _unassignedRequest);
            Assert.AreSame(sortedList.ElementAt(2), _completedRequest);
        }


        [Test]
        public void SortRequests_Descending_ByDefault()
        {
            var sortedList = _requestService.SortRequests("Not A Prop", "desc", _requestList);

            Assert.AreSame(sortedList.ElementAt(2), _assignedRequest);
            Assert.AreSame(sortedList.ElementAt(1), _unassignedRequest);
            Assert.AreSame(sortedList.ElementAt(0), _completedRequest);
        }

        [Test]
        public void SortRequests_Ascending_ByDefault()
        {
            var sortedList = _requestService.SortRequests("Not A Prop", "asc", _requestList);

            Assert.AreSame(sortedList.ElementAt(0), _assignedRequest);
            Assert.AreSame(sortedList.ElementAt(1), _unassignedRequest);
            Assert.AreSame(sortedList.ElementAt(2), _completedRequest);
        }



        private string GetDisplayName(string memberName)
        {
            MemberInfo[] memberInfos = (typeof (Request)).GetMember(memberName);

            if (!memberInfos.Any())
                throw new Exception("Property was not found");

            MemberInfo memberInfo = memberInfos[0];
            DisplayNameAttribute displayNameAttribute =
                (DisplayNameAttribute) Attribute.GetCustomAttribute(memberInfo, typeof (DisplayNameAttribute));

            if (displayNameAttribute == null)
                return memberInfo.Name;

            return displayNameAttribute.DisplayName;
        }
    }
}