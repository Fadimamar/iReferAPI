using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;
namespace iReferAPI.Models
{
    public class AgencyRole : Record
    {
        public Agency Agency { get; set; }
        [ForeignKey("Users")]
        public string EmployeeUserID { get; set; }
        [ForeignKey("Agency")]
        public string AgencyId { get; set; }
        [ForeignKey("Role")]
        public int UserRoleID { get; set; }
    }
}
