using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace iReferAPI.Models
{
   
    public class AgencyRequest
    {

        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string AgencyName { get; set; }
        [StringLength(100)]
        public string Website { get; set; }
        [Required]
        [StringLength(20)]
        public string PhoneNo { get; set; }


        [StringLength(50)]
        public string Address1 { get; set; }
        [StringLength(50)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string City { get; set; }
        [StringLength(10)]
        public string ZipCode { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        public IFormFile Logo { get; set; }
    }

    public class AccountRequest
    {
        public string Id { get; set; }

        [Required]
        [StringLength(300)]
        public string ReferralURL { get; set; }
        public string ReferralCode { get; set; }

        public string AgencyId { get; set; }
    }
  
}
