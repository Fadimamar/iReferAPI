using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace iReferAPI.Models
{
    
   

    public  class AgencyInvitation:Record
        {

            
            
        [Required]
            [StringLength(100)]
            public string Email { get; set; }

            [Column(TypeName = "date")]
            public DateTime? DateInvited { get; set; }
    
        public bool IsSent { get; set; }
        public bool IsViewed { get; set; }
        public bool HasSubscribed { get; set; }
        public Agency Agency { get; set; }
        [ForeignKey("Agency")]
        public string AgencyId { get; set; }
    }

   

           
        }


    


