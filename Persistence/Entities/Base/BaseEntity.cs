using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities;

public class BaseEntity : ITimestamp
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}

public interface ISoftDeletable
{
    public DateTime? DeletedAt { get; set; }
}

public interface ITimestamp
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}