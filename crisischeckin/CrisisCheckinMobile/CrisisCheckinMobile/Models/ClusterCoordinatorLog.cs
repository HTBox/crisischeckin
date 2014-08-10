using Breeze.Sharp;
using System;

namespace CrisisCheckinMobile.Models
{
    public class ClusterCoordinatorLogEntry : BaseEntity
    {
        public int Id { get { return GetValue<int>(); } set { SetValue(value); } }
        public DateTime TimeStampUtc { get { return GetValue<DateTime>(); } set { SetValue(value); } }
        public ClusterCoordinatorEvents Event { get { return GetValue<ClusterCoordinatorEvents>(); } set { SetValue(value); } }
        public int PersonId { get { return GetValue<int>(); } set { SetValue(value); } }
        public string PersonName { get { return GetValue<string>(); } set { SetValue(value); } }
        public int DisasterId { get { return GetValue<int>(); } set { SetValue(value); } }
        public string DisasterName { get { return GetValue<string>(); } set { SetValue(value); } }
        public int ClusterId { get { return GetValue<int>(); } set { SetValue(value); } }
        public string ClusterName { get { return GetValue<string>(); } set { SetValue(value); } }
    }
}