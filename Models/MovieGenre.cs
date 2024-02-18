using System.ComponentModel.DataAnnotations.Schema;

namespace QFX.Models;

public class MovieGenre
{
    public long ID { get; set; }
    public long MovieID { get; set; }
    [ForeignKey("MovieID")]
    public virtual Movie? Movie { get; set; }
    public long GenreID { get; set; }
    [ForeignKey("GenreID")]
    public virtual Genre? Genre { get; set; }
}
