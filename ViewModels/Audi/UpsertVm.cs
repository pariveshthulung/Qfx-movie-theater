using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;

namespace QFX.ViewModels.AudiVm;

public class UpsertVm
{
    public string? Name { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public int PremiumRow { get; set; }
    public  List<Models.Location>? Locations { get; set; }
    public long LocationID { get; set; }
    public SelectList LocationList(){
        return new SelectList(
            Locations,
            nameof(Models.Location.ID),
            nameof(Models.Location.CityName),
            LocationID
        );
    }
}
