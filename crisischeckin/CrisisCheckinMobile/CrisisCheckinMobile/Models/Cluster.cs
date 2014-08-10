using Breeze.Sharp;
namespace CrisisCheckinMobile.Models
{
    public class Cluster : BaseEntity
    {
        public int Id { get { return GetValue<int>(); } set { SetValue(value); } }
        public string Name { get { return GetValue<string>(); } set { SetValue(value); } }
    }
}
