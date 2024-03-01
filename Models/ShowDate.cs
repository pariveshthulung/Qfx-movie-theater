using System.ComponentModel.DataAnnotations.Schema;

namespace QFX.Models;

public class ShowDate
{
    public long? ID { get; set; }
    public DateTime Date { get; set; }
    public long  ShowID { get; set; }
    [ForeignKey("ShowID")]
    public virtual Show? Show { get; set; }
}
