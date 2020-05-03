using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace iReferAPI.Models
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; }

    }
}
