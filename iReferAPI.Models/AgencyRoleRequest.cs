using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace iReferAPI.Models
{
    public enum UserTypes { SysAdmin, AgencyAdmin = 2, AgencyUser = 3 }
    public class AgencyRoleRequest
    {
       
        public string AgencyRoleID { get; set; }
        [Required]
        public string EmployeeUserID { get; set; }

        [Required]
        public string AgencyID { get; set; }
        public UserTypes UserType { get; set; }

       
    }
}
