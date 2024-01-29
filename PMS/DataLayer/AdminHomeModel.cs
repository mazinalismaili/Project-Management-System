using PMS.Models;

namespace PMS.DataLayer
{
    public class AdminHomeModel
    {
        public IEnumerable<UserModel> Users { get; set; }
        public IEnumerable<Role> Roles { get; set;}
        public IEnumerable<Status> Statuses { get; set; }
        public IEnumerable<Priority> Priorities { get; set; }
        public IEnumerable<UserRolesModel> UserRoles { get; set; }

        //public IEnumerable<Models.ProjectModel> Projects { get; set; }
        
    }
}
