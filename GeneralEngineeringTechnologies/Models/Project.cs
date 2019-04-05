using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeneralEngineeringTechnologies.Models
{
    public class Project
    {
        public int Id { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Project name is required.")]
        public string Name { get; set; }

        public string Code { get; set; }

        [Display(Name = "Project Manager")]
        public ApplicationUser ProjectManager { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}