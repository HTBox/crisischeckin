using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class RequestIndexPageViewModel
    {
        public IEnumerable<Request> Requests { get; set; }
        public RequestSearch RequestSearch { get; set; }
    }

    public class RequestSearch : Request
    {
        [DisplayName("Request Status")]
        public RequestStatus RequestStatus { get; set; }

        public DateTime? NullableEndDate { get; set; }
        public DateTime? NullableCreatedDate { get; set; }
    }

    public enum RequestStatus
    {
        All,
        Unassigned,
        Assigned,
        Completed
    }
}