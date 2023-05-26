using Microsoft.AspNetCore.Identity;

namespace MVC.Models;

public class UserEntity : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public string Address { get; set; }
}