using System.ComponentModel.DataAnnotations.Schema;

namespace QFX.Models;

public class Location
{
    public long ID { get; set; }
    public string? CityName { get; set; }
    public long AudiID { get; set; }
    [ForeignKey("AudiID")]
    public virtual Audi? Audi { get; set; }
}
