using System.ComponentModel.DataAnnotations.Schema;

namespace D.Models;

[Table("SS")]
public class SS
{
    [Column(TypeName = "nvarchar(900)")]
    public string Id { get; set; } = string.Empty;

    [Column(TypeName = "varbinary(MAX)")]
    public byte[] Value { get; set; } = [];

    [Column(TypeName = "datetimeoffset(7)")]
    public DateTimeOffset ExpiresAtTime { get; set; }

    [Column(TypeName = "bigint")]
    public long? SlidingExpirationInSeconds { get; set; }

    [Column(TypeName = "datetimeoffset(7)")]
    public DateTimeOffset? AbsoluteExpiration { get; set; }
}