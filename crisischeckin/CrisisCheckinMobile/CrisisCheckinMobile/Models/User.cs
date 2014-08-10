using Breeze.Sharp;
namespace CrisisCheckinMobile.Models
{
    public class User : BaseEntity
    {
        public int Id { get { return GetValue<int>(); } set { SetValue(value); } }
        public string UserName { get { return GetValue<string>(); } set { SetValue(value); } }
    }
}
