using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iReferAPI.Models
{

    public class Account : Record
    {

        [Required]
        [StringLength(300)]
        public string ReferralURL { get; set; }

        
       
        public string ReferralCode { get; set; }

        public Agency Agency { get; set; }

        [ForeignKey("Agency")]
        public string AgencyId { get; set; }
    }
}
