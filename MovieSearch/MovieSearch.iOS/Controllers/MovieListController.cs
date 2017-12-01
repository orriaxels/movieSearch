using System;
using System.Collections.Generic;
using System.Text;
using MovieSearch.Model;
using MovieSearch.Services;
using MovieDownload;
using UIKit;
using System.Threading.Tasks;
using MovieSearch.iOS.Controllers;

namespace MovieSearch.iOS.Controllers
{
    public class MovieListController : UITableViewController
    {
        private List<MovieDetails> _movieList;
        private ImageDownloader _imageDownloader;
        private MovieService _api;

        public MovieListController(ImageDownloader imageDownloader, MovieService api)
        {
            this._api = api;
            this._imageDownloader = imageDownloader;
            this._movieList = _api.GetMovies();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.Title = "Movie list";
            this.TableView.Source = new MovieListDataSource(this._movieList, _onSelectedMovie);
            GetAllCastMembers(_movieList);
            //GetMovieDetail(_movieList);
            DownloadPosters(_movieList);
        }

        private void _onSelectedMovie(int row)
        {
            MovieDetails movie = this._movieList[row];
            NavigationItem.BackBarButtonItem = new UIBarButtonItem("Movie list", UIBarButtonItemStyle.Plain, null);
            this.NavigationController.PushViewController(new MovieDetailController(movie, this._api), true);
        }

        private async void DownloadPosters(List<MovieDetails> movies)
        {
            await _imageDownloader.DownloadImagesInList(movies);
            this.TableView.ReloadData();
        }

        private async void GetAllCastMembers(List<MovieDetails> movies)
        {
            foreach(MovieDetails movie in movies)
            {
                var cast = await _api.GetCreditList(movie.id);
                movie.actors = cast;
                this.TableView.ReloadData();
            }
        }
    }
}
