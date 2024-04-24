using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class AccountViewModel
    {
        public string FName { get; set; }
        public string LName { get; set; }

        [EmailAddress(ErrorMessage ="Invalid Email")]
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Password is required")]
        public string Password {  get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password",ErrorMessage ="Password doesn't match")]
        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }
        public bool  IsAgree { get; set; }

    }
}
