using GeneralEngineeringTechnologies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeneralEngineeringTechnologies.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Project Project { get; set; }

        public ApplicationUser AssignedUser { get; set; }

        public DateTime Deadline { get; set; }

        public int Proggres { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }
    }
}