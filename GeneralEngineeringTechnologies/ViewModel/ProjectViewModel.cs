using GeneralEngineeringTechnologies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeneralEngineeringTechnologies.ViewModel
{
    public class ProjectViewModel
    {
        public Project Project { get; set; }

        public IEnumerable<ApplicationUser> ProjectManagers { get; set; }

        public string ProjectManager { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}