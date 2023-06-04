using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVC.DTOs;


public class UserDTO : BaseDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    [RegularExpression(@"^\d*$")]
    public string PhoneNumber { get; set; }
    [Required]
    public string Address { get; set; }
}

public class CreateUserDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
    public string UserName { get; set; }
    [Required]
    [RegularExpression(@"^\d*$")]
    public string PhoneNumber { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public string Address { get; set; }
}

public class UpdateUserDTO
{
    public string Email { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}
