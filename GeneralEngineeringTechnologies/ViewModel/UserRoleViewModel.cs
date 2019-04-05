using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeneralEngineeringTechnologies.ViewModel
{
    public class UserRoleViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string Id { get; set; }
    }
}