namespace QFX.ViewModels.PublicVm;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QFX.Models;

public class IndexVm
{
    public List<Show>? NowShowing { get; set; }
    public List<Show>? Shows { get; set; }
    public long? LocationID { get; set; }
    public List<Location>? Locations { get; set; }
    public SelectList LocationList(){
        return new SelectList(
            Locations,
            nameof(Models.Location.ID),
            nameof(Models.Location.CityName),
            LocationID
        );
    }

}
