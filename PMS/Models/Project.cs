using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PMS.Models
{
    public class Project
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Project Name")]

        public string Name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }

        /*[Required]
        [StringLength(450)]
        [Display(Name = "Project Status")]
        public string Status { get; set; }*/

        [Required]
        [StringLength(450)]
        [Display(Name = "Project Description")]
        public string Description { get; set; }

        public string? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public string? StatusId { get; set; }
        public Status? Status { get; set; }

        public ICollection<Task>? Tasks{ get; set; }


    }
}   
