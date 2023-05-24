using Microsoft.EntityFrameworkCore;
namespace WebApi.Entities;

[Index(nameof(PhoneNumber), IsUnique = true)]
public class UserInfoEntity : BaseEntity
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public virtual UserEntity User { get; set; }
}