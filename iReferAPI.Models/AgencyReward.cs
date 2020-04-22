using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Collections.Generic;


namespace iReferAPI.Models
{
    public class AgencyReward:Record
    {


        public Agency Agency { get; set; }
        public int RewardReviewDays { get; set; }
        [ForeignKey("Agency")]
        public string AgencyId { get; set; }
        public string RewardType { get; set; }
        public ICollection<CouponReward> CouponRewards { get; set; }
        public ICollection<CustomReward> CustomRewards { get; set; }
        public ICollection<PointReward> PointRewards { get; set; }
    }
}
