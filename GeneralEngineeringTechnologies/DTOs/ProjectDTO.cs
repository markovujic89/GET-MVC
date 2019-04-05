using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GeneralEngineeringTechnologies.DTOs
{
    public class ProjectDTO
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public string Code { get; set; }

        public ICollection<GeneralEngineeringTechnologies.Models.Task> Tasks { get; set; }
    }
}