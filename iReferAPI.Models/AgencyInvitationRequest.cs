using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace iReferAPI.Models
{
    
    

    public  class AgencyInvitationRequest      
    
    
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string AgencyId { get; set; }
        [Required]
        public string EnrollPageURL { get; set; }
    }

   

           
        }


    


