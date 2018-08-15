using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace project0_7.Models
{
    public class CustomerLoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [Key]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }

    public class CustomerPasswordResetViewModel : CustomerLoginViewModel
    {
        CustomerPasswordResetViewModel() : base()
        {
        }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [Key]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [MaxLength(50)]
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Required]
        [MaxLength(50)]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Zip Code")]
        public string Zip { get; set; }

        [Required]
        [MaxLength(50)]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string City { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Phone]
        [Display(Name ="Phone")]
        public string Phone { get; set; }
    }
}