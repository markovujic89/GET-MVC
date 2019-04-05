using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeneralEngineeringTechnologies.ViewModel
{
    public class GroupedUserViewModel
    {
        public List<UserRoleViewModel> Users { get; set; }
        public List<UserRoleViewModel> ProjectManager { get; set; }
    }
}