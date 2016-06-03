using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class RequestIndexPageViewModel
    {
        public List<Request> Requests { get; set; }
        public RequestSearch RequestSearch { get; set; }
    }

    public class RequestSearch : Request
    {
        public Status Status { get; set; }
    }

    public enum Status
    {
        Unassigned,
        Assigned,
        Closed
    }
}