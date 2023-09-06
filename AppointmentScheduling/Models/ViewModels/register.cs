using System.ComponentModel.DataAnnotations;

namespace AppointementScheduling.Models.ViewModels
{
    public class register
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]   
        [StringLength(100,ErrorMessage ="the{0} must be at least {2} characters long",MinimumLength = 6)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [Compare("Password",ErrorMessage ="the password and Confirmation password do not match.")]
        public string Confirmpassword { get; set; }
        [Required]
        [Display(Name="Role Name")]
        public string RoleName { get; set; }
    }
}
