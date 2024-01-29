using System.ComponentModel.DataAnnotations;

namespace PMS.Models
{
    public class Priority
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Priority")]
        public string Name { get; set; }

        public ICollection<Task>? Task { get; set; }

    }
}
