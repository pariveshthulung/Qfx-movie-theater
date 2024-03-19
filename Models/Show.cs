using System.ComponentModel.DataAnnotations.Schema;
using QFX.Manager.Interface;

namespace QFX.Models;

public class Show : ISoftDelete
{
    public long ID { get; set; }
    public string? ShowStatus { get; set; }
    public long MovieID { get; set; }
    [ForeignKey("MovieID")]
    public virtual Movie? Movie { get; set; }
    public long AudiID { get; set; }
    [ForeignKey("AudiID")]
    public virtual Audi? Audi { get; set; }
    public bool IsDeleted { get; set; } = false;
}
