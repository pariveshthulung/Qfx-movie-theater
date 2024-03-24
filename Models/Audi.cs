using System.ComponentModel.DataAnnotations.Schema;
using QFX.Manager.Interface;

namespace QFX.Models;

public class Audi : ISoftDelete
{
    public string? Name { get; set; }
    public long ID { get; set; }
    public int Row { get; set; }
    public int PremiumRow { get; set; }
    public int Column { get; set; }
    public long LocationID { get; set; }
    [ForeignKey("LocationID")]
    public virtual Location? Location { get; set; }
    
    public bool IsDeleted { get; set; } = false;
}
