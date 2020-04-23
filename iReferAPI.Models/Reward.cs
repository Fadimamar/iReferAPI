using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace iReferAPI.Models
{
    public enum RewardTypes { Cash=1, Coupon = 2, Custom = 3, Points = 4 }

    public  class Reward:Record
        {

            
            
        [Required]
            [StringLength(100)]
            public string Description { get; set; }

            [Column(TypeName = "date")]
            public DateTime? ExpirationDate { get; set; }
        [Required]
        public RewardTypes RewardType { get; set; }
      
        public int RewardReviewDays { get; set; }
       
         [Required]
         [StringLength(500)]
         public string Message { get; set; }

        public float DiscountRate { get; set; }

        public bool NoExpiration { get; set; }
        public int? PointsAmount { get; set; }
        [Column(TypeName = "money")]
        public decimal? EquivalentDollarAmount { get; set; }
        [Column(TypeName = "money")]
        public decimal? CashAmount { get; set; }
        public Agency Agency { get; set; }
        [ForeignKey("Agency")]
        public string AgencyId { get; set; }
    }

   

           
        }


    


