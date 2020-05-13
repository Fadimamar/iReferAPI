using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace iReferAPI.Models
{
    
    public enum OfferTypes { Link=1, Coupon = 2 }

    public  class FriendOffer:Record
        {

            
            
        [Required]
            [StringLength(100)]
            public string Description { get; set; }

            [Column(TypeName = "date")]
            public DateTime? ExpirationDate { get; set; }
        [Required]
        public OfferTypes OfferType { get; set; }
      
              
         [Required]
         [StringLength(500)]
         public string Message { get; set; }

        public float DiscountRate { get; set; }

        public bool NoExpiration { get; set; }
        [StringLength(100)]
        public string LandingPage { get; set; }
        public string SalesPhoneNumber { get; set; }
        public Agency Agency { get; set; }
        [ForeignKey("Agency")]
        public string AgencyId { get; set; }
    }

   

           
        }


    


