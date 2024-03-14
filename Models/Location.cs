using System.ComponentModel.DataAnnotations.Schema;

namespace QFX.Models;

public class Location
{
    public long ID { get; set; }
    public string? CityName { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal PreminumPrice { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal PlatinumPrice { get; set; }

}
