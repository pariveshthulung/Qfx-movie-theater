﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QFX.Models;

namespace QFX.ViewModels.Movie;

public class MovieUpdateVm
{
    public string? MovieTitle { get; set; }
    public string? Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? Runtime { get; set; }
     public string? Cast { get; set; }
    public string? Director { get; set; }
    public IFormFile? PosterImage { get; set; }
    public IFormFile? CoverImage { get; set; }
    public string? ImageUrl { get; set; }
    public string? CoverUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public List<Language>? Languages;
    public long LanguageID { get; set; }
    public List<Genre>? Genres ;
    public long GenreID { get; set; }
    public List<long>? GenreIDs { get; set; }

    public SelectList GenreList(){
        return new SelectList(
            Genres,
            nameof(Genre.ID),
            nameof(Genre.Name),
            GenreID
        );
    }
    public SelectList LanguageList(){
        return new SelectList(
            Languages,
            nameof(Language.ID),
            nameof(Language.Name),
            LanguageID
        );
    }

}
