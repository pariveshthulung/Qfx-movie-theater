using QFX.Models;

namespace QFX.ViewModels;

public class MovieIndexVm
{
    public List<Models.Movie>? Movies { get; set; }
    public List<MovieGenre>? MovieGenres { get; set; }
    public Language? Language { get; set; }
}
