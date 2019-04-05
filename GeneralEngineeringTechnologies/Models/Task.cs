using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeneralEngineeringTechnologies.Models
{
    public class Task
    {
        public int Id { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Pleas, enter the task's name.")]
        public string Name { get; set; }

        public Project Project { get; set; }

        [Display(Name = "Assigned User")]
        //[MaximumThreeAssignedTask()]
        public ApplicationUser AssignedUser { get; set; }

        public DateTime Deadline { get; set; }

        public int Progress { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }


    }
}