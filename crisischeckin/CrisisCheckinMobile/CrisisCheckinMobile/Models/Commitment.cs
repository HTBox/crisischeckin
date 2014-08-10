using Breeze.Sharp;
using System;

namespace CrisisCheckinMobile.Models
{
    public class Commitment : BaseEntity
    {
        public int Id { get { return GetValue<int>(); } set { SetValue(value); } }
        public int PersonId { get { return GetValue<int>(); } set { SetValue(value); } }
        public DateTime StartDate { get { return GetValue<DateTime>(); } set { SetValue(value); } }
        public DateTime EndDate { get { return GetValue<DateTime>(); } set { SetValue(value); } }
        public int DisasterId { get { return GetValue<int>(); } set { SetValue(value); } }
        public bool PersonIsCheckedIn { get { return GetValue<bool>(); } set { SetValue(value); } }

        public Disaster Disaster { get { return GetValue<Disaster>(); } set { SetValue(value); } }
        public Person Person { get { return GetValue<Person>(); } set { SetValue(value); } }
    }
}
