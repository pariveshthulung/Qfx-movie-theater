using System.ComponentModel.DataAnnotations.Schema;

namespace QFX.Models;

public class Audi
{
    public string? Name { get; set; }
    public long ID { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public long LocationID { get; set; }
    [ForeignKey("LocationID")]
    public virtual Location? Location { get; set; }
}
