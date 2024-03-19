using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using QFX.Constants;
using QFX.data;
using QFX.Entity;
using QFX.Models;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[AllowAnonymous]
public class SeederController : Controller
{
    private readonly ApplicationDbContext _context;
    public SeederController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> SeedAdmin()
    {
        try
        {

            var AdminExist = await _context.Users.AnyAsync(x => x.UserType == UserTypeConstants.Admin);
            if (!AdminExist)
            {
                using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var admin = new User
                {
                    Name = "Admin",
                    Email = "QfxAdmin@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                    UserType = UserTypeConstants.Admin
                };
                await _context.AddAsync(admin);
                await _context.SaveChangesAsync();
                tx.Complete();
                return Content("admin added");
            }
            return Content("Admin exist");
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
    }

    public async Task<IActionResult> SeedGenre()
    {
        try
        {
            var GenreExist = await _context.Locations.AnyAsync();
            if (!GenreExist)
            {
                using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                List<Models.Genre> genres = new List<Models.Genre>()
                {
                    new() {Name ="Action"},
                    new() {Name ="Thriller"},
                    new() {Name ="Drama"},
                    new() {Name ="Horror"},
                    new() {Name ="Drama"},
                    new() {Name ="Science fiction"},
                    new() {Name ="Crime film"},
                    new() {Name ="Western"},
                    new() {Name ="Animation"},
                    new() {Name ="Fantasy"},
                    new() {Name ="Television"},
                    new() {Name ="Romance"},
                    new() {Name ="Comedy"},
                    new() {Name ="Historical Fiction"},
                    new() {Name ="Romance"},
                    new() {Name ="Comedy"},
                    new() {Name ="Musical genre"},
                    new() {Name ="Comedic genres"},
                    new() {Name ="Experimental"},
                    new() {Name ="Documentary"},
                    new() {Name ="Musical"},
                    new() {Name ="Narrative"},
                    new() {Name ="Crime"},
                    new() {Name ="Fiction"},
                    new() {Name ="Science fiction"},
                    new() {Name ="Fantasy"},
                    new() {Name ="Mystery"},
                    new() {Name ="Action fiction"},
                    new() {Name ="Action/Adventure"},
                    new() {Name ="Adventure"},
                    new() {Name ="Satire"},
                    new() {Name ="History"},
                    new() {Name ="Noir"},
                    new() {Name ="Narrative"},
                    new() {Name ="Thriller"},
                    new() {Name ="Short"},
                    new() {Name ="Hindi cinema"},
                    new() {Name ="Mystery"},
                    new() {Name ="Romantic comedy"},
                    new() {Name ="Historical drama"},
                    new() {Name ="Animated film"},
                    new() {Name ="Historical film"},
                    new() {Name ="Suspense"},
                    new() {Name ="War"},
                    new() {Name ="Melodrama"},
                    new() {Name ="Music"},
                    new() {Name ="Exploitation"},
                    new() {Name ="Science"},
                    new() {Name ="Epic"},
                    new() {Name ="Disaster"},
                    new() {Name ="Art"}
                };
                await _context.Genres.AddRangeAsync(genres);
                await _context.SaveChangesAsync();
                tx.Complete();
                return Content("Added Genre!!!");
            }
            return Content("Genre exist!!!");
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
    }
    public async Task<IActionResult> SeedLanguage()
    {
        try
        {
            var LanguageExist = await _context.Languages.AnyAsync();
            if (!LanguageExist)
            {
                using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                List<Models.Language> languages = new List<Models.Language>()
                {
                    // new() {Id = -1, Name ="English"},
                    // new() {Id = -2, Name ="Nepali"},
                    new() {Name ="Hindi"},
                    new() {Name ="Japanese"},

                };
                await _context.Languages.AddRangeAsync(languages);
                await _context.SaveChangesAsync();
                tx.Complete();
                return Content("Added Language!!!");
            }
            return Content("language exist!!!");
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
    }

    public IActionResult SeedMovie()
    {
        try
        {
            // using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            // List<Movie> movies = new()
            // {
            //     new() {
            //         ID=1,
            //         Title="Spider-Man: Far from Home",
            //             Description= "Peter Parker and his friends go on a summer trip to Europe. However, they will hardly be able to rest - Peter will have to agree to help Nick Fury uncover the mystery of creatures that cause natural disasters and destruction throughout the continent." ,
            //             Cast = "Tom Holland,Samuel L. Jackson,Zendaya,Cobie Smulders,Jon Favreau" ,
            //             Director="Jon Watts",
            //             TrailerUrl="Nt9L1jCKGnE",
            //             },
            //     new() {
            //         ID=2,
            //         Title="Avengers: Infinity War",
            //             Description= "As the Avengers and their allies have continued to protect the world from threats too large for any one hero to handle, a new danger has emerged from the cosmic shadows: Thanos. A despot of intergalactic infamy, his goal is to collect all six Infinity Stones, artifacts of unimaginable power, and use them to inflict his twisted will on all of reality. Everything the Avengers have fought for has led up to this moment - the fate of Earth and existence itself has never been more uncertain." ,
            //             Cast = "Tom Holland,Samuel L. Jackson,Zendaya,Cobie Smulders,Jon Favreau" ,
            //             Director="Jon Watts",
            //             TrailerUrl="Nt9L1jCKGnE",
            //             }
                // new() {Title="The Basketball Diaries",
                //         Description= "A high school basketball player’s life turns upside down after free-falling into the harrowing world of drug addiction." ,
                //         Cast = "Leonardo DiCaprio,Samuel L. Jackson,Zendaya,Cobie Smulders,Jon Favreau" ,
                //         Director="Scott Kalvert",
                //         TrailerUrl="Nt9L1jCKGnE",
                //         },
                // new() {Title="The Godfather",
                //         Description= "Spanning the years 1945 to 1955, a chronicle of the fictional Italian-American Corleone crime family. When organized crime family patriarch, Vito Corleone barely survives an attempt on his life, his youngest son, Michael steps in to take care of the would-be killers, launching a campaign of bloody revenge." ,
                //         Cast = "Francis Ford Coppola,Samuel L. Jackson,Zendaya,Cobie Smulders,Jon Favreau", 
                //         Director="Francis Ford Coppola",
                //         TrailerUrl="Nt9L1jCKGnE",
                //         },
                // new() {Title="The Terminator",
                //         Description= "In the post-apocalyptic future, reigning tyrannical supercomputers teleport a cyborg assassin known as the 'Terminator' back to 1984 to kill Sarah Connor, whose unborn son is destined to lead insurgents against 21st century mechanical hegemony. Meanwhile, the human-resistance movement dispatches a lone warrior to safeguard Sarah. Can he stop the virtually indestructible killing machine?" ,
                //         Cast = "Tom Holland,Samuel L. Jackson,Zendaya,Cobie Smulders,Jon Favreau" ,
                //         Director="Jon Watts",
                //         TrailerUrl="Nt9L1jCKGnE",
                //         },
                // new() {Title="Die Hard",
                //         Description= "Peter Parker and his friends go on a summer trip to Europe. However, they will hardly be able to rest - Peter will have to agree to help Nick Fury uncover the mystery of creatures that cause natural disasters and destruction throughout the continent." ,
                //         Cast = "Tom Holland,Samuel L. Jackson,Zendaya,Cobie Smulders,Jon Favreau", 
                //         Director="Jon Watts",
                //         TrailerUrl="Nt9L1jCKGnE",
                //         },
                // new() {Title="Live Free or Die Hard",
                //         Description= "Peter Parker and his friends go on a summer trip to Europe. However, they will hardly be able to rest - Peter will have to agree to help Nick Fury uncover the mystery of creatures that cause natural disasters and destruction throughout the continent." ,
                //         Cast = "Tom Holland,Samuel L. Jackson,Zendaya,Cobie Smulders,Jon Favreau", 
                //         Director="Jon Watts",
                //         TrailerUrl="Nt9L1jCKGnE",
                //         }
            // };
             _context.Movies.AddRange(new List<Movie> (){
                new() {Title="The Basketball Diaries",
                        Description= "A high school basketball player’s life turns upside down after free-falling into the harrowing world of drug addiction." ,
                        Cast = "Leonardo DiCaprio,Samuel L. Jackson,Zendaya,Cobie Smulders,Jon Favreau" ,
                        Director="Scott Kalvert",
                        TrailerUrl="Nt9L1jCKGnE",
                        LanguageID = -1,
                        
                        },
                new() {Title="The Godfather",
                        Description= "Spanning the years 1945 to 1955, a chronicle of the fictional Italian-American Corleone crime family. When organized crime family patriarch, Vito Corleone barely survives an attempt on his life, his youngest son, Michael steps in to take care of the would-be killers, launching a campaign of bloody revenge." ,
                        Cast = "Francis Ford Coppola,Samuel L. Jackson,Zendaya,Cobie Smulders,Jon Favreau", 
                        Director="Francis Ford Coppola",
                        TrailerUrl="Nt9L1jCKGnE",
                        LanguageID = -1
                        },
             });
                _context.SaveChanges();
                // tx.Complete();
            return Content("Movie Added");
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
    }
}


