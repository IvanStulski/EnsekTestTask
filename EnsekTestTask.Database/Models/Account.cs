using System.ComponentModel.DataAnnotations;

namespace EnsekTestTask.Database.Models;

public class Account
{
    [Key]
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public virtual ICollection<Meter> Meters { get; set; } = null!;
}
