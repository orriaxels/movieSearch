using System;
using System.Collections.Generic;
using CoreGraphics;
using MovieSearch.Model;
using MovieSearch.Services;
using MovieSearch.iOS.Controllers;
using UIKit;
using MovieDownload;
using System.Threading;

namespace MovieSearch.iOS
{
    public partial class MovieSearchViewController : UIViewController
    {
        private MovieService _api;
        private ImageDownloader _imageDownloader;
        private const double StartX = 20;
        private const double StartY = 80;
        private const double Height = 50;
        public string resultTitle;
        private List<MovieDetails> _movieList;
        private UIActivityIndicatorView uiActivityIndicator;

        public MovieSearchViewController(MovieService api, ImageDownloader imageDownloader)
        {
            _api = api;
            _imageDownloader = imageDownloader;
            _movieList = new List<MovieDetails>();
            uiActivityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
            this.TabBarItem = new UITabBarItem(UITabBarSystemItem.Search, 0);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.Title = "Movie Search";
            this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = UIColor.White
            };
            this.NavigationController.NavigationBar.BarTintColor = UIColor.FromRGB(57, 57, 57);
            this.NavigationController.NavigationBar.TintColor = UIColor.White;
            this.View.BackgroundColor = UIColor.FromRGB(48, 48, 48);

            var promptLabel = PromptLabel();
            var titleField = UiTextField();
            var resultField = ResultLabel();
            var searchButton = SearchButton(titleField);
            this.uiActivityIndicator.Frame = new CGRect(this.View.Bounds.Width / 2, this.View.Bounds.Height / 2, 0, 0);

            this.View.AddSubviews(new UIView[] { titleField, resultField, searchButton, uiActivityIndicator });
        }

        // Helper functions
        private UILabel PromptLabel()
        {
            var promptLabel = new UILabel()
            {
                Frame = new CGRect(StartX, StartY, this.View.Bounds.Width - 2 * StartX, Height),
                Text = "Enter words in movie title:",
            };

            return promptLabel;
        }

        private UILabel ResultLabel()
        {
            var resultlabel = new UILabel()
            {
                Frame = new CGRect(StartX, StartY + 3 * Height, this.View.Bounds.Width - 2 * StartX, Height),
                Text = ""
            };

            return resultlabel;
        }

        private UITextField UiTextField()
        {
            var nameField = new UITextField()
            {
                Frame = new CGRect(StartX, StartY + Height, this.View.Bounds.Width - 2 * StartX, Height),
                BorderStyle = UITextBorderStyle.RoundedRect,
                Placeholder = "Enter movie title..."
            };
            return nameField;
        }

        private UIButton SearchButton(UITextField titleField)
        {
            var getMovieButton = UIButton.FromType(UIButtonType.RoundedRect);
            getMovieButton.Layer.CornerRadius = 7;
            getMovieButton.Frame = new CGRect(StartX, StartY + 3 * Height, this.View.Bounds.Width - 2 * StartX, Height);
            getMovieButton.SetTitle("Get movie", UIControlState.Normal);
            getMovieButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            getMovieButton.BackgroundColor = UIColor.FromRGB(2,119,189);
            getMovieButton.Font = UIFont.SystemFontOfSize(18);

            getMovieButton.TouchUpInside += async (sender, args) =>
            {
                uiActivityIndicator.StartAnimating();
                titleField.ResignFirstResponder();
                resultTitle = titleField.Text;

                _movieList = await _api.GetMovieByTitle(resultTitle);
                

                NavigationItem.BackBarButtonItem = new UIBarButtonItem("Movie search", UIBarButtonItemStyle.Plain, null);
                this.NavigationController.PushViewController(new MovieListController(this._imageDownloader, this._api), true);

                uiActivityIndicator.StopAnimating();
                resultTitle = "";
                titleField.Text = "";
            };

            return getMovieButton;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

