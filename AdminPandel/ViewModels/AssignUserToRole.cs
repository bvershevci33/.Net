using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.ViewModels
{
    public class AssignUserToRole
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public bool IsInRole { get; set; }
    }
}
