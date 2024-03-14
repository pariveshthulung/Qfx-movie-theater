using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Models;
using QFX.ViewModels;
using QFX.ViewModels.Movie;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin,Employee")]
public class MovieController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notifyService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MovieController(ApplicationDbContext context, INotyfService notyfService, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _notifyService = notyfService;
        _webHostEnvironment = webHostEnvironment;
    }
    // method to add and update image
    public string ConfirmImage(IFormFile imageFile, string imageExist)
    {
        if (imageFile != null)
        {

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(wwwRootPath, @"Public/images/movie");
            string imagePath = Path.Combine(filePath, fileName);

            // update image
            if (!string.IsNullOrEmpty(imageExist))
            {
                var oldImagePath = imagePath;
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            // insert image
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }
            // image name
            return fileName;
        }
        return ("error adding image");
    }
    public IActionResult Index(MovieIndexVm vm)
    {
        vm.Movies = _context.Movies
        .Include(x => x.MovieGenres)
            .ThenInclude(y => y.Genre)
        .Include(x => x.Language).ToList();
        return View(vm);
    }

    public async Task<IActionResult> Add()
    {
        var vm = new MovieAddVm();
        vm.Genres = await _context.Genres.ToListAsync();
        vm.Languages = await _context.Languages.ToListAsync();

        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Add(MovieAddVm vm)
    {
        try
        {
            var movie = new Movie
            {
                Title = vm.MovieTitle,
                TrailerUrl = vm.TrailerUrl,
                Description = vm.Description,
                ReleaseDate = vm.ReleaseDate,
                Runtime = TimeSpan.Parse(vm.Runtime),
                LanguageID = vm.LanguageID,
                Cast = vm.Cast,
                Director = vm.Director
            };
            var imageName = ConfirmImage(vm.PosterImage, movie.ImageUrl);
            var coverName = ConfirmImage(vm.CoverImage,movie.CoverUrl);
            movie.CoverUrl = coverName;
            movie.ImageUrl = imageName;
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            foreach (var ids in vm.GenreIDs)
            {
                var movieGenre = new MovieGenre();
                movieGenre.GenreID = ids;
                movieGenre.MovieID = movie.ID;
                await _context.MovieGenres.AddAsync(movieGenre);
                await _context.SaveChangesAsync();
            }
            _notifyService.Success("Product added successfully.");
            return RedirectToAction("Index");

        }
        catch (Exception e)
        {
            _notifyService.Error("Operation failed." + e.Message);
            return RedirectToAction("Add");
        }
    }
    public async Task<IActionResult> Update(long ID)
    {
        var vm = new MovieUpdateVm();
        var movie = await _context.Movies.FirstOrDefaultAsync(x => x.ID == ID);

        vm.MovieTitle = movie.Title;
        vm.ImageUrl = movie.ImageUrl;
        vm.CoverUrl = movie.CoverUrl;
        vm.TrailerUrl = movie.TrailerUrl;
        vm.Runtime = movie.Runtime.ToString();
        vm.Description = movie.Description;
        vm.LanguageID = movie.LanguageID;
        vm.Cast = movie.Cast;
        vm.Director = movie.Director;
        vm.GenreIDs = _context.MovieGenres.Where(x => x.MovieID == movie.ID).Select(x => x.GenreID).ToList();
        vm.Genres = _context.Genres.ToList();
        vm.Languages = _context.Languages.ToList();

        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(long ID, MovieUpdateVm vm)
    {
        var movie = _context.Movies.FirstOrDefault(x => x.ID == ID);
        movie.Title = vm.MovieTitle;
        movie.Description = vm.Description;
        if (vm.PosterImage != null)
        {
            var imageName = ConfirmImage(vm.PosterImage, movie.ImageUrl);
            movie.ImageUrl = imageName;
        }
        if(vm.CoverImage != null){
            var coverName = ConfirmImage(vm.CoverImage,movie.CoverUrl);
            movie.CoverUrl = coverName;
        }
        movie.LanguageID = vm.LanguageID;
        movie.Runtime = TimeSpan.Parse(vm.Runtime);
        movie.TrailerUrl = vm.TrailerUrl;
        movie.Cast = vm.Cast;
        movie.Director = vm.Director;

        await _context.SaveChangesAsync();

        // List<long> oldMovieGenre = _context.MovieGenres.Where(x => x.MovieID == movie.ID).Select(x => x.GenreID).ToList();
        _context.MovieGenres.RemoveRange(_context.MovieGenres.Where(x => x.MovieID == movie.ID));
        await _context.SaveChangesAsync();
        foreach (var ids in vm.GenreIDs)
        {
            var movieGenre = new MovieGenre();
            movieGenre.GenreID = ids;
            movieGenre.MovieID = movie.ID;
            await _context.MovieGenres.AddAsync(movieGenre);
            await _context.SaveChangesAsync();
        }
        _notifyService.Success("Product added successfully.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(long ID)
    {
        try
        {

            _context.Movies.Remove(_context.Movies.Where(x=>x.ID==ID).FirstOrDefault());
            List<MovieGenre> movieGenres = _context.MovieGenres.Where(x => x.MovieID == ID).ToList();
            _context.MovieGenres.RemoveRange(movieGenres);
            await _context.SaveChangesAsync();
            _notifyService.Success("Deleted sucessfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            _notifyService.Error("Error deleting" + e.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
