using System.ComponentModel.DataAnnotations.Schema;

namespace QFX.Models;

public class Show
{
    public long ID { get; set; }
    public DateTime Time { get; set; }
    public DateTime Date { get; set; }
    public string? ShowStatus { get; set; }
    public long MovieID { get; set; }
    [ForeignKey("MovieID")]
    public virtual Movie? Movie { get; set; }
    public long AudiID { get; set; }
    [ForeignKey("AudiID")]
    public virtual Audi? Audi { get; set; }
}
