using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using QFX.Constants;
using QFX.data;
using QFX.Entity;
using QFX.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Storage;



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
                using (var transaction = _context.Database.BeginTransaction())
                {
                    // using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                    List<Models.Genre> genres = new List<Models.Genre>()
                    {
                        new() {ID=-1 ,Name ="Action"},
                        new() {ID=-2 ,Name ="Thriller"},
                        new() {ID=-3 ,Name ="Drama"},
                        new() {ID=-4 ,Name ="Horror"},
                        new() {ID=-5 ,Name ="Drama"},
                        new() {ID=-6 ,Name ="Science fiction"},
                        new() {ID=-7 ,Name ="Crime film"},
                        new() {ID=-8 ,Name ="Western"},
                        new() {ID=-9 ,Name ="Animation"},
                        new() {ID=-10 ,Name ="Fantasy"},
                        new() {ID=-11 ,Name ="Television"},
                        new() {ID=-12 ,Name ="Romance"},
                        new() {ID=-13 ,Name ="Comedy"},
                        new() {ID=-14 ,Name ="Historical Fiction"},
                        new() {ID=-15 ,Name ="Romance"},
                        new() {ID=-16 ,Name ="Comedy"},
                        new() {ID=-17 ,Name ="Musical genre"},
                        new() {ID=-18 ,Name ="Comedic genres"},
                        new() {ID=-19 ,Name ="Experimental"},
                        new() {ID=-20 ,Name ="Documentary"},
                        new() {ID=-21 ,Name ="Musical"},
                        new() {ID=-22 ,Name ="Narrative"},
                        new() {ID=-23 ,Name ="Crime"},
                        new() {ID=-24 ,Name ="Fiction"},
                        new() {ID=-25 ,Name ="Science fiction"},
                        new() {ID=-26 ,Name ="Fantasy"},
                        new() {ID=-27 ,Name ="Mystery"},
                        new() {ID=-28 ,Name ="Action fiction"},
                        new() {ID=-29 ,Name ="Action/Adventure"},
                        new() {ID=-30 ,Name ="Adventure"},
                        new() {ID=-31 ,Name ="Satire"},
                        new() {ID=-32 ,Name ="History"},
                        new() {ID=-33 ,Name ="Noir"},
                        new() {ID=-34 ,Name ="Narrative"},
                        new() {ID=-35 ,Name ="Thriller"},
                        new() {ID=-36 ,Name ="Short"},
                        new() {ID=-37 ,Name ="Hindi cinema"},
                        new() {ID=-38 ,Name ="Mystery"},
                        new() {ID=-39 ,Name ="Romantic comedy"},
                        new() {ID=-40 ,Name ="Historical drama"},
                        new() {ID=-41 ,Name ="Animated film"},
                        new() {ID=-42 ,Name ="Historical film"},
                        new() {ID=-43 ,Name ="Suspense"},
                        new() {ID=-44 ,Name ="War"},
                        new() {ID=-45 ,Name ="Melodrama"},
                        new() {ID=-46 ,Name ="Music"},
                        new() {ID=-47 ,Name ="Exploitation"},
                        new() {ID=-48 ,Name ="Science"},
                        new() {ID=-49 ,Name ="Epic"},
                        new() {ID=-50 ,Name ="Disaster"},
                        new() {ID=-51 ,Name ="Art"}
                    };
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[Genres] ON;");
                    _context.Genres.AddRange(genres);
                    await _context.SaveChangesAsync();
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Genres] OFF;");
                    transaction.Commit();
                    return Content("Added Genre!!!");
                }
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
                // using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using (var transaction = _context.Database.BeginTransaction())
                {
                    List<Models.Language> languages = new List<Models.Language>()
                {
                    new() { ID=-1, Name ="English"},
                    new() { ID=-2, Name ="Nepali"},
                    new() {ID = -3, Name ="Hindi"},
                    new() { ID = -4,Name ="Japanese"}

                };
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[Languages] ON;");
                    await _context.Languages.AddRangeAsync(languages);
                    await _context.SaveChangesAsync();
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Languages] OFF;");
                    // tx.Complete();
                    transaction.Commit();
                }
                return Content("Added Language!!!");
            }
            return Content("language exist!!!");
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
    }


    public async Task<IActionResult> SeedMovie()
    {
        try
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[Movies] ON;");
                _context.Movies.AddRange(new List<Movie>(){
                new() {
                        ID = -1,
                        Title="John Wick",
                        Description= "With the untimely death of his beloved wife still bitter in his mouth, John Wick, the expert former assassin, receives one final gift from her--a precious keepsake to help John find a new meaning in life now that she is gone. But when the arrogant Russian mob prince, Iosef Tarasov, and his men pay Wick a rather unwelcome visit to rob him of his prized 1969 Mustang and his wife's present, the legendary hitman will be forced to unearth his meticulously concealed identity. Blind with revenge, John will immediately unleash a carefully orchestrated maelstrom of destruction against the sophisticated kingpin, Viggo Tarasov, and his family, who are fully aware of his lethal capacity. Now, only blood can quench the boogeyman's thirst for retribution." ,
                        Cast = "Keanu Reeves,Michael Nyqvist,Alfie Allen,Adrianne Palicki,Bridget Moynahan,Dean Winters,Ian McShane,John Leguizamo,Willem Dafoe" ,
                        Director="	Chad Stahelski",
                        TrailerUrl="qEVUtrk8_B4",
                        LanguageID = -1,
                        ReleaseDate = DateTime.Now.AddDays(2),
                        Runtime = new TimeSpan(2, 45, 00),
                        ImageUrl = "62c965f2-8158-4548-948e-82d343011141.jpg",
                        CoverUrl = ""

                        },
                new() {
                        ID = -2,
                        Title="Interstellar",
                        Description= "When Earth becomes uninhabitable in the future, a farmer and ex-NASA pilot, Joseph Cooper, is tasked to pilot a spacecraft, along with a team of researchers, to find a new planet for humans." ,
                        Cast = "Matthew McConaughey,Anne Hathaway,Jessica Chastain,Bill Irwin,Ellen Burstyn,Michael Caine",
                        Director="Christopher Nolan",
                        TrailerUrl="zSWdZVtXT7E",
                        LanguageID = -1,
                        ReleaseDate =  DateTime.Now.AddDays(2),
                        Runtime = new TimeSpan(2, 45, 00),
                        ImageUrl = "f9e69e14-3da6-4c63-ae43-93eaa18f9da1.jpg",
                        CoverUrl = ""
                        },
                new() {
                        ID= -3,
                        Title="The Avenger : Infinity war",
                        Description= "The Avengers and their allies must be willing to sacrifice all in an attempt to defeat the powerful Thanos before his blitz of devastation and ruin puts an end to the universe." ,
                        Cast = "Robert Downey Jr.,Chris Hemsworth,Mark Ruffalo,Chris Evans,Scarlett Johansson,Benedict Cumberbatch,Don Cheadle,Tom Holland" ,
                        Director="Anthony Russo",
                        TrailerUrl="6ZfuNTqbHE8",
                        ReleaseDate = DateTime.Now.AddDays(2),
                        Runtime = new TimeSpan(2, 45, 00),
                        LanguageID = -1,
                        ImageUrl = "f0f7e72e-6b2b-4740-a9ba-fed488592bf5.jpeg",
                        CoverUrl = ""
                        },

                new() {
                        ID=-4,
                        Title="The Spiderman: No way home",
                        Description= "With Spider-Man's identity now revealed, Peter asks Doctor Strange for help. When a spell goes wrong, dangerous foes from other worlds start to appear, forcing Peter to discover what it truly means to be Spider-Man." ,
                        Cast = "Tom Holland,Zendaya,Benedict Cumberbatch,Jacob Batalon,Jon Favreau,Jamie Foxx",
                        Director="Francis Ford Coppola",
                        TrailerUrl="JfVOs4VSpmA",
                        ReleaseDate = DateTime.Now.AddDays(2),
                        Runtime = new TimeSpan(2, 45, 00),
                        LanguageID = -1,
                        ImageUrl = "e5abf235-dd65-47d4-9201-6f85bb58e419.jpg",
                        CoverUrl = ""
                        },
                new() {
                        ID=-5,
                        Title="Django Unchained",
                        Description= "With the help of a German bounty-hunter, a freed slave sets out to rescue his wife from a brutal plantation owner in Mississippi." ,
                        Cast = "Jamie Foxx,Christoph Waltz,Leonardo DiCaprio,Kerry Washington,Samuel L. Jackson" ,
                        Director="Quentin Tarantino",
                        TrailerUrl="0fUCuvNlOCg",
                        ReleaseDate = DateTime.Now.AddDays(2),
                        Runtime = new TimeSpan(2, 45, 00),
                        LanguageID = -1,
                        ImageUrl = "c85d0135-41eb-4ca4-bad7-77854e25ea39.jpg",
                        CoverUrl = ""
                        },
                new() {
                        ID=-6,
                        Title="Bob Marley : One love",
                        Description= "The story of how reggae icon Bob Marley overcame adversity, and the journey behind his revolutionary music." ,
                        Cast = "Kingsley Ben-Adir,Lashana Lynch,James Norton",
                        Director="Reinaldo Marcus Green",
                        TrailerUrl="gaGEmMCGLBU",
                        ReleaseDate = DateTime.Now.AddDays(2),
                        Runtime = new TimeSpan(2, 45, 00),
                        LanguageID = -1,
                        ImageUrl = "37d503b4-7963-446e-9160-06188696f4a1.jpeg",
                        CoverUrl = "a8564c58-484e-4e31-a4a5-f5c1ee3f5452.jpg"
                        },
                new() {
                        ID=-7,
                        Title="Despicable me 4",
                        Description= "Gru, Lucy, Margo, Edith, and Agnes welcome a new member to the family, Gru Jr., who is intent on tormenting his dad. Gru faces a new nemesis in Maxime Le Mal and his girlfriend Valentina, and the family is forced to go on the run." ,
                        Cast = "Steve Carell,Kristen Wiig,Will Ferrell,Joey King",
                        Director="Chris Renaud",
                        TrailerUrl="qQlr9-rF32A",
                        LanguageID = -1,
                        ReleaseDate = DateTime.Now.AddDays(2),
                        Runtime = new TimeSpan(2, 45, 00),
                        ImageUrl = "a77dd7f9-1e9d-47a6-9edf-cb24571d8c57.webp",
                        CoverUrl = ""
                    }
             });

                await _context.SaveChangesAsync();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[Movies] OFF;");
                transaction.Commit();
            }
            return Content("Movie Added");
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
    }
    public async Task<IActionResult> SeedLocation()
    {
        try
        {
            using (var transaction = _context.Database.BeginTransaction())
            {

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[Locations] ON;");

                _context.Locations.AddRange(new List<Models.Location>(){
                new(){
                    ID = -1,
                    CityName = "Birtamode",
                    PreminumPrice = 420,
                    PlatinumPrice = 280
                },
                new(){
                    ID = -2,
                    CityName = "Damak",
                    PreminumPrice = 420,
                    PlatinumPrice = 280
                },
                new(){
                    ID = -3,
                    CityName = "Birathnagar",
                    PreminumPrice = 420,
                    PlatinumPrice = 280
                },
                new(){
                    ID = -4,
                    CityName = "Kathmandu",
                    PreminumPrice = 420,
                    PlatinumPrice = 280
                },
                new(){
                    ID = -5,
                    CityName = "Butwal",
                    PreminumPrice = 420,
                    PlatinumPrice = 280
                },
                new(){
                    ID = -6,
                    CityName = "Pokhara",
                    PreminumPrice = 420,
                    PlatinumPrice = 280
                },
            });
                await _context.SaveChangesAsync();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[Locations] OFF;");
                transaction.Commit();
                return Content("Locations added");
            }
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
    }
    public async Task<IActionResult> SeedAudi()
    {
        try
        {
            using (var transaction = _context.Database.BeginTransaction())
            {


                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[Audis] ON;");
                _context.Audis.AddRange(new List<Audi>(){
                new(){
                    ID = -1,
                    Name = "Audi1",
                    Row = 6,
                    Column = 8,
                    LocationID = -1
                },
                new(){
                    ID = -2,
                    Name = "Audi2",
                    Row = 6,
                    Column = 8,
                    LocationID = -1
                },
                new(){
                    ID = -3,
                    Name = "Audi1",
                    Row = 6,
                    Column = 8,
                    LocationID = -2
                },
                new(){
                    ID = -4,
                    Name = "Audi2",
                    Row = 6,
                    Column = 8,
                    LocationID = -2
                },
                new(){
                    ID = -5,
                    Name = "Audi1",
                    Row = 6,
                    Column = 8,
                    LocationID = -3
                },
                new(){
                    ID = -6,
                    Name = "Audi2",
                    Row = 6,
                    Column = 8,
                    LocationID = -3
                },
                new(){
                    ID = -7,
                    Name = "Audi1",
                    Row = 9,
                    Column = 10,
                    LocationID = -4
                },
                new(){
                    ID = -8,
                    Name = "Audi2",
                    Row = 9,
                    Column = 11,
                    LocationID = -4
                },
                new(){
                    ID = -9,
                    Name = "Audi1",
                    Row = 6,
                    Column = 8,
                    LocationID = -5
                },
                new(){
                    ID = -10,
                    Name = "Audi2",
                    Row = 7,
                    Column = 8,
                    LocationID = -5
                },
                new(){
                    ID = -11,
                    Name = "Audi1",
                    Row = 6,
                    Column = 8,
                    LocationID = -6
                },
                new(){
                    ID = -12,
                    Name = "Audi2",
                    Row = 6,
                    Column = 9,
                    LocationID = -6
                },
                new(){
                    ID = -13,
                    Name = "Audi3",
                    Row = 8,
                    Column = 10,
                    LocationID = -6
                },
            });
                await _context.SaveChangesAsync();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[Audis] OFF;");
                transaction.Commit();
                return Content("Audi added!!");
            }
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }

    }
    public async Task<IActionResult> SeedSeatAsync()
    {
        try
        {
            using (var transaction = _context.Database.BeginTransaction())
            {

                // _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[Seats] ON;");

                var audis = _context.Audis.ToList();
                foreach (var audi in audis)
                {
                    char x = 'A';
                    for (var i = 1; i <= audi.Column; i++)
                    {
                        for (var j = 1; j <= audi.Row; j++)
                        {

                            var seat = new Seat();
                            seat.SeatName = x.ToString() + j;
                            seat.AudiID = audi.ID;
                            if (i == (audi.Column - 2))
                            {
                                seat.SeatType = SeatTypeConstants.Premium;
                            }
                            _context.Seats.Add(seat);
                            await _context.SaveChangesAsync();
                        }
                        x++;
                    }
                }
                await _context.SaveChangesAsync();
                // _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[Seats] OFF;");
                transaction.Commit();
                return Content("Seats added!!");
            }
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }

    }
    public async Task<IActionResult> SeedShowAsync()
    {
        try
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var ids = -1;
                for (var i = 1; i <= 13; i++)
                {
                    for (var j = 1; j <= 7; j++)
                    {

                        var show = new Show
                        {
                            ID = ids,
                            MovieID = -j,
                            AudiID = -i
                        };
                        if (j <= 3)
                        {
                            show.ShowStatus = ShowStatusConstants.NowShowing;
                        }
                        else
                        {
                            show.ShowStatus = ShowStatusConstants.ComingSoon;
                        }
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[Shows] ON;");
                        _context.Shows.Add(show);
                        await _context.SaveChangesAsync();
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[Shows] OFF;");

                        //  add date 
                        if (j <= 3)
                        {
                            DateTime WholeDate = DateTime.Now.AddDays(2);
                            WholeDate = WholeDate.AddTicks(-(WholeDate.Ticks % TimeSpan.TicksPerSecond));
                            var date = new ShowDate();
                            date.ID = ids;
                            date.Date = WholeDate;
                            date.ShowID = ids;
                            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[ShowDates] ON;");
                            _context.ShowDates.Add(date);
                            await _context.SaveChangesAsync();
                            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[ShowDates] OFF;");

                            //  add time in each date

                            var time = new ShowTime();
                            time.ID = ids;
                            time.ShowDateID = ids;
                            time.Time = new DateTime(2024, 10, 12, 6, 30, 0);

                            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[ShowTimes] ON;");
                            _context.ShowTimes.Add(time);
                            await _context.SaveChangesAsync();
                            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[ShowTimes] OFF;");

                            // add showSeat 
                            // for (var a = 1; a <= 3; a++)
                            // {
                            // var seatId = _context.Seats.Where(x => x.AudiID == ids).Select(X => X.ID).ToList();
                            // var seatId = _context.Seats.Select(X => X.ID).ToList();
                            // foreach (var id in seatId)
                            // {
                            //     var showSeat = new ShowSeat();
                            //     showSeat.ShowTimeID = ids;
                            //     showSeat.SeatID = id;
                            //     showSeat.ShowSeatStatus = SeatStatusConstants.Active;
                            //     // _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[ShowSeats] ON;");
                            //     _context.ShowSeats.Add(showSeat);
                            //     await _context.SaveChangesAsync();
                            //     // _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT  [dbo].[ShowSeats] OFF;");

                            // }
                            // }


                        }

                        ids--;
                    }


                }

                var ShowTimeDB = _context.ShowTimes.Include(x=>x.ShowDate).ThenInclude(y => y.Show).ToList();

                foreach (var showTimeDB in ShowTimeDB)
                {
                    var seatId = _context.Seats.Where(x=>x.AudiID == showTimeDB.ShowDate.Show.AudiID).Select(X => X.ID).ToList();
                    foreach (var id in seatId)
                    {
                        var showSeat = new ShowSeat();
                        showSeat.ShowTimeID = showTimeDB.ID;
                        showSeat.SeatID = id;
                        showSeat.ShowSeatStatus = SeatStatusConstants.Active;
                        _context.ShowSeats.Add(showSeat);
                        await _context.SaveChangesAsync();
                    }
                }


                await _context.SaveChangesAsync();
                transaction.Commit();
                return Content("Shows,ShowDate,ShowTime and ShowSeats added!!");
            }
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
    }
}


