using System.ComponentModel.DataAnnotations;

namespace PMS.Models
{
    public class Status
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name="Status")]
        public string Name { get; set; }

        //public string ProjectId { get; set; }
        public ICollection<Project>? Project { get; set; }

        //public string TaskId { get; set; }
        public ICollection<Task>? Task { get; set; }


    }
}
