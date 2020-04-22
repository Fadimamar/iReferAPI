using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace iReferAPI.Models
{
    
        public  class Reward:Record
        {

                       
        [Required]
            [StringLength(100)]
            public string Description { get; set; }

            [Column(TypeName = "date")]
            public DateTime? ExpirationDate { get; set; }

           

            [Required]
            [StringLength(500)]
            public string Message { get; set; }

            public bool NoExpiration { get; set; }
           

             public Agency AgencyReward { get; set; }

            [ForeignKey("AgencyReward")]
            public string AgencyRewardId { get; set; }
        }

    public class CustomReward : Reward
    {



        public string RewardType()
        {
            return "Custom";
        }
    }
        public class CouponReward : Reward
        {



            public string RewardType()
            {
                return "Coupon";
            }


            public float DiscountRate { get; set; }

        }

        public class PointReward : Reward
        {



            public string RewardType()
            {
                return "Points";
            }
            public int? PointsAmount { get; set; }

            [Column(TypeName = "money")]
            public decimal? EquivalentDollarAmount { get; set; }

        }


    
}

