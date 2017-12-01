using System.Collections.Generic;
using UIKit;

using MovieSearch.Services;
using MovieSearch.Model;
using MovieDownload;
using System;
using CoreGraphics;

namespace MovieSearch.iOS.Controllers
{
    public class TopTenController : UITableViewController
    {
        private MovieService _api;
        private ImageDownloader _imageDownloader;
        private UIActivityIndicatorView uiActivityIndicator;
        private List<MovieDetails> _movieList;
        public bool download;

        public TopTenController(MovieService api, ImageDownloader imageDownloader)
        {
            this._api = api;
            this._imageDownloader = imageDownloader;
            this._movieList = new List<MovieDetails>();
            this.TabBarItem = new UITabBarItem(UITabBarSystemItem.TopRated, 1);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.Title = "Top rated movies";
            this.TableView.RowHeight = 106;
            this.View.BackgroundColor = UIColor.White;
            uiActivityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);

            this.TableView.Source = new MovieListDataSource(this._movieList, _onSelectedMovie);

            this.uiActivityIndicator.Frame = new CGRect(this.View.Bounds.Width / 2, 70, 0, 0);
            this.View.AddSubviews(new UIView[] { uiActivityIndicator });

            //download = true;
        }

        public async override void ViewDidAppear(Boolean animated)
        {
            base.ViewDidAppear(animated);

            if(download)
            {
                this.uiActivityIndicator.StartAnimating();
                this._movieList = await _api.getTopRatedMovies();
                DownloadPosters(this._movieList);

                foreach (MovieDetails movie in this._movieList)
                {
                    await _api.GetCreditList(movie);
                }

                this.TableView.Source = new MovieListDataSource(this._movieList, _onSelectedMovie);
                this.TableView.ReloadData();
                this.uiActivityIndicator.StopAnimating();
            }
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
        }

        private async void GetAllCastMembers(List<MovieDetails> movies)
        {
            foreach (MovieDetails movie in movies)
            {
                await _api.GetCreditList(movie);
            }
            this.TableView.ReloadData();
        }
    }
}
