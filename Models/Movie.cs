namespace QFX.Models;

public class Movie
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public TimeSpan Runtime { get; set; }
    public string TrailerUrl { get; set; }
}
