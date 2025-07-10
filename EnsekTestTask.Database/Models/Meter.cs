using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EnsekTestTask.Database.Models;

public class Meter
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public DateTime MeterReadingDateTime { get; set; }
    public int MeterReadValue { get; set; }

    public long AccountId { get; set; }
    public virtual Account Account { get; set; } = null!;
}

public class MeterEqulityComparer : IEqualityComparer<Meter>
{
    public bool Equals(Meter? x, Meter? y)
    {
        return x.MeterReadValue == y.MeterReadValue
            && x.AccountId == y.AccountId
            && x.MeterReadingDateTime == y.MeterReadingDateTime;
    }

    public int GetHashCode([DisallowNull] Meter obj)
    {
        return obj?.GetHashCode() ?? 0;
    }
}
