using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PMS.Models
{
    public class Employee : IdentityUser
    {
        [StringLength(450)]
        [Display(Name ="First Name")]
        public string FirstName {  get; set; }

        [StringLength(450)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public bool Gender {  get; set; }
        public bool isManager { get; set; } = false;

        public string? ManagerId {  get; set; }
        public Employee? Manager {  get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
