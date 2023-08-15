using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MovieDemo.Models;
using MovieDemo.Models.Repository;
using MovieDemo.ViewModels;
using NToastNotify;
using System.Data;

namespace MovieDemo.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieDemoRepo<Movie> _movieRepo;
        private readonly IMovieDemoRepo<Genre> _genreRepo;
        private readonly IToastNotification _toastNotification;
        private new List<string> _allowedext = new List<string> { ".jpg", ".png" };
        private long _allowedPosterSize = 5242880;

        public MoviesController(IMovieDemoRepo<Movie> movieRepo, IMovieDemoRepo<Genre> genreRepo, IToastNotification toastNotification)
        {
            _movieRepo = movieRepo;
            _genreRepo = genreRepo;
            _toastNotification = toastNotification;
        }

        // GET: MoviesController
        public ActionResult Index()
        {
            var movies = _movieRepo.GetAll().OrderByDescending(m => m.Rate);
            return View(movies);
        }

        // GET: MoviesController/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();
            
            var movie  = _movieRepo.Get(id);

            if (movie == null)
                return NotFound(nameof(movie));

            return View(movie);
        }

        // GET: MoviesController/Create
        public ActionResult Create()
        {
            var viewModel = new MovieFormViewModel
            {
                Genres = _genreRepo.GetAll().OrderBy(g => g.Name),
            };
            return View("MovieForm",viewModel);
        }

        // POST: MoviesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = _genreRepo.GetAll().OrderBy(m => m.Name);
                return View("MovieForm", model);
            }

            var files = Request.Form.Files;

            if (!files.Any())
            {
                model.Genres = _genreRepo.GetAll().OrderBy(m => m.Name);
                ModelState.AddModelError("Poster", "Please select movie poster!");
                return View("MovieForm", model);
            }

            var poster = files.FirstOrDefault();

            if (!_allowedext.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.Genres = _genreRepo.GetAll().OrderBy(m => m.Name);
                ModelState.AddModelError("Poster", "Only .PNG, .JPG images are allowed!");
                return View("MovieForm", model);
            }

            if (poster.Length > _allowedPosterSize)
            {
                model.Genres = _genreRepo.GetAll().OrderBy(m => m.Name);
                ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB!");
                return View("MovieForm", model);
            }


            using var dataStream = new MemoryStream();
            poster.CopyTo(dataStream);

            var movie = new Movie
            {
                Title = model.Title,
                Description = model.Description,
                Rate = model.Rate,
                Year = model.Year,
                GenreID = (byte)model.GenreID,
                Poster = dataStream.ToArray()
            };


            _movieRepo.Add(movie);

            _toastNotification.AddSuccessToastMessage("Movie Created Successfully!");

            return RedirectToAction(nameof(Index));
        }

        // GET: MoviesController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
                return BadRequest();

            var movie = _movieRepo.Get(id);
            if (movie == null)
                return NotFound();

            var viewModel = new MovieFormViewModel
            {
                Id = movie.MovieID,
                Title = movie.Title,
                Description = movie.Description,
                Rate = movie.Rate,
                Year = movie.Year,
                GenreID = movie.GenreID,
                Poster = movie.Poster,
                Genres = _genreRepo.GetAll().OrderBy(m => m.Name),
            };
            return View("MovieForm", viewModel);
        }

        // POST: MoviesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MovieFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _genreRepo.GetAll().OrderBy(g => g.Name);

                return View("MovieForm", viewModel);
            }

            var movie = _movieRepo.Get(id);
            if (movie == null)
                return NotFound();

            var files = Request.Form.Files;
            if (files.Any())
            {
                var poster = files.SingleOrDefault();

                using var dataStream = new MemoryStream();

                poster.CopyTo(dataStream);

                viewModel.Poster = dataStream.ToArray();

                if (!_allowedext.Contains(Path.GetExtension(poster.FileName).ToLower()))
                {
                    viewModel.Genres = _genreRepo.GetAll().OrderBy(m => m.Name);
                    ModelState.AddModelError("Poster", "Only .PNG, .JPG images are allowed!");
                    return View("MovieForm", viewModel);
                }

                if (poster.Length > _allowedPosterSize)
                {
                    viewModel.Genres = _genreRepo.GetAll().OrderBy(m => m.Name);
                    ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB!");
                    return View("MovieForm", viewModel);
                }

                movie.Poster = dataStream.ToArray();
            }


            movie.Title = viewModel.Title;
            movie.Description = viewModel.Description;
            movie.Rate = viewModel.Rate;
            movie.Year = viewModel.Year;
            movie.GenreID = (byte)viewModel.GenreID;

            _movieRepo.Save();

            _toastNotification.AddSuccessToastMessage("Movie Updated Successfully!");

            return RedirectToAction(nameof(Index));
        }

        // GET: MoviesController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            _movieRepo.Delete(id);
            
            return Ok();
        }

        // POST: MoviesController/Delete/5
    }
}
