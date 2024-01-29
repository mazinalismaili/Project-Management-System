namespace PMS.Models
{
    public class UserRolesModel
    {
        public Employee Employee { get; set; }
        public IList<string> Roles { get; set;}
    }
}
