namespace QFX.ViewModels.Movie;

public class MovieAddVm
{
    public string? MovieTitle { get; set; }
    public string? Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public TimeSpan Runtime { get; set; }
    public string? ImageUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public long LanguageID { get; set; }
   
    public long GenreID { get; set; }

}
