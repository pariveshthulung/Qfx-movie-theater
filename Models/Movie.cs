using System.ComponentModel.DataAnnotations.Schema;
using QFX.Manager.Interface;

namespace QFX.Models;

public class Movie : ISoftDelete
{
    public long ID { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public TimeSpan Runtime { get; set; }
    public string? ImageUrl { get; set; }
    public string? CoverUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public long LanguageID { get; set; }
    [ForeignKey("LanguageID")]
    public virtual Language? Language { get; set; }
    public string? Cast { get; set; }
    public string? Director { get; set; }
    public virtual List<MovieGenre>? MovieGenres  { get; set; }
    public bool IsDeleted { get; set; } = false;

}
