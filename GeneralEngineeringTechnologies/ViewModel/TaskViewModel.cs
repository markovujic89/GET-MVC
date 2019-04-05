using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeneralEngineeringTechnologies.Models;
using System.Web;
using Task = GeneralEngineeringTechnologies.Models.Task;

namespace GeneralEngineeringTechnologies.ViewModel
{
    public class TaskViewModel
    {
        public Task Task { get; set; }

        public string ProjectName { get; set; }

        public string UserName { get; set; }
    }
}