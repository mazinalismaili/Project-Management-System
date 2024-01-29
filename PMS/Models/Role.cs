using System.ComponentModel.DataAnnotations;

namespace PMS.Models
{
    public class Role
    {
        public string Id { set; get; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Role")]
        public string Name { set; get; }

    }
}
