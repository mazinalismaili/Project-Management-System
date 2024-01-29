using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Models
{
    public class Task
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Task Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /*[Required]
        [StringLength(450)]
        [Display(Name = "Status")]
        public string Status { get; set; }
*/
        /*[Required]
        [StringLength(450)]
        [Display(Name = "Priority")]
        public string Priority { get; set; }*/

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Task Starting Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Task Ending Date")]
        public DateTime EndDate { get; set; }

        public string? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public string? ProjectId { get; set; }
        public Project? Project { get; set; }

        public string? StatusId { get; set; }
        public Status? Status { get; set; }

        public string? PriorityId { get; set; }
        public Priority? Priority { get; set; }


        public ICollection<Comment>? Comments { get; set;}
    }
}
