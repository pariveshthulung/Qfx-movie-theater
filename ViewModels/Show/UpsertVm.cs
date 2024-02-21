using Microsoft.AspNetCore.Mvc.Rendering;
using QFX.Constants;

namespace QFX.ViewModels.ShowVm;

public class UpsertVm
{
    public DateTime Time { get; set; }
    public DateTime Date { get; set; }
    public string? ShowStatus { get; set; }
    // public List<ShowStatusConstants>? ShowStatusConstants { get; set; }
    public long MovieID { get; set; }
    public long AudiID { get; set; }
    public List<Models.Movie>? Movies  { get; set; }
    public List<Models.Audi>? Audis  { get; set; }
    public SelectList ShowStatusList(){
        return new SelectList(
            ShowStatusConstants.SelectStatus,
            ShowStatus
        );
    }
    public SelectList MovieList(){
        return new SelectList(
            Movies,
            nameof(Models.Movie.ID),
            nameof(Models.Movie.Title),
            MovieID
        );
    }

    public SelectList AudiList(){
        return new SelectList(
            Audis,
            nameof(Models.Audi.ID),
            nameof(Models.Audi.Location),
            AudiID
        );
    }

}
