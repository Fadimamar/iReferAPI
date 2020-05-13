using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iReferAPI.Models
{
   public  class OnlineOfferRequest
    {
        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpirationDate { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        public float DiscountRate { get; set; }

        public bool NoExpiration { get; set; }
        [StringLength(100)]
        public string LandingPage { get; set; }
        public string SalesPhoneNumber { get; set; }
        
        public string AgencyId { get; set; }
    }
   public class CouponOfferRequest
    {
        [Required]

        public string Description { get; set; }


        public DateTime? ExpirationDate { get; set; }
       
        public int RewardReviewDays { get; set; }

        [Required]

        public string Message { get; set; }

        
        public bool NoExpiration { get; set; }
        public float DiscountRate { get; set; }

        public string AgencyId { get; set; }
    }
    
}
