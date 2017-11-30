using System;
using System.Collections.Generic;
using CoreGraphics;
using MovieSearch.Model;
using MovieSearch.iOS.Controllers;
using UIKit;
using MovieDownload;
using System.Threading;

namespace MovieSearch.iOS
{
    public partial class MovieSearchViewController : UIViewController
    {
        private MyMovieApi _api;
        private ImageDownloader _imageDownloader;
        private const double StartX = 20;
        private const double StartY = 80;
        private const double Height = 50;
        public string resultTitle;
        private List<MovieDetails> _movieList;
        private UIActivityIndicatorView uiActivityIndicator;

        public MovieSearchViewController(MyMovieApi api, ImageDownloader imageDownloader)
        {
            _api = api;
            _imageDownloader = imageDownloader;
            _movieList = new List<MovieDetails>();
            uiActivityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.BackgroundColor = UIColor.White;
            NavigationItem.Title = "Movie Search";
            var promptLabel = PromptLabel();
            var titleField = UiTextField();
            var resultField = ResultLabel();
            var searchButton = SearchButton(titleField, resultField);

            uiActivityIndicator.Frame = new CGRect(this.View.Bounds.Width / 2, this.View.Bounds.Height / 2, 0, 0);

            this.View.AddSubviews(new UIView[] { promptLabel, titleField, resultField, searchButton, uiActivityIndicator });
        }

        // Helper functions
        private UILabel PromptLabel()
        {
            var promptLabel = new UILabel()
            {
                Frame = new CGRect(StartX, StartY, this.View.Bounds.Width - 2 * StartX, Height),
                Text = "Enter words in movie title:"
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
                BorderStyle = UITextBorderStyle.RoundedRect
            };
            return nameField;
        }

        private UIButton SearchButton(UITextField titleField, UILabel resultField)
        {
            var getMovieButton = UIButton.FromType(UIButtonType.RoundedRect);
            getMovieButton.Frame = new CGRect(StartX, StartY + 2 * Height, this.View.Bounds.Width - 2 * StartX, Height);
            getMovieButton.SetTitle("Get movie", UIControlState.Normal);

            getMovieButton.TouchUpInside += async (sender, args) =>
            {
                uiActivityIndicator.StartAnimating();
                titleField.ResignFirstResponder();
                resultTitle = titleField.Text;

                if (resultTitle != "")
                {
                    var result = await _api.GetMovieByTitle(resultTitle);

                    foreach (MovieDetails info in result)
                    {
                        var localPath = _imageDownloader.LocalPathForFilename(info.imageUrl);

                        await _imageDownloader.DownloadImage(info.imageUrl, localPath, CancellationToken.None);    
                        info.posterFilePath = localPath;
                    }

                    _movieList = result;
                }
                else
                {
                    this._movieList.Clear();
                }

                NavigationItem.BackBarButtonItem = new UIBarButtonItem("Movie search", UIBarButtonItemStyle.Plain, null);
                this.NavigationController.PushViewController(new MovieListController(this._movieList), true);
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

