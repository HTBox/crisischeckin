namespace Models
{
    public class VolunteerType
    {
        public const string VOLUNTEERTYPE_ONSITE = "On Site";
        public const string VOLUNTEERTYPE_REMOTE = "Remote";

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
