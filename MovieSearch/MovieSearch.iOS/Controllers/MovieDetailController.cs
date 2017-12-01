using System;
using MovieSearch.Model;
using UIKit;
using Foundation;
using CoreGraphics;
using System.Text.RegularExpressions;
using System.Text;
using MovieSearch.Services;

namespace MovieSearch.iOS.Controllers
{
    public class MovieDetailController : UIViewController
    {
        private MovieDetails _movie;
        private MovieService _api;
        private const double topPadding = 70;
        private const double headerTitleSize = 30;
        private const double descriptionSize = 200;
        private const double framePadding = 10;
        private const double xPadding = 10;
        private const double yPadding = 10;
        private const double Height = 200;
        private const double posterEndX = 153;
        private const double ImageHeight = 192;
        private const double ImageWidth = 128;

        private UIImageView _imageView;
        private UILabel _description;
        private UIView _shadow;
        private UILabel _titleHeader;
        private UILabel _runtime;
        private UILabel _year;
        private UILabel _genres;
        private UIView _line;

        public MovieDetailController(MovieDetails movie, MovieService api)
        {
            this._movie = movie;
            this._api = api;
            GetMovieDetail(this._movie);

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View.BackgroundColor = UIColor.White;
            NavigationItem.Title = "Movie Detail";

            _titleHeader = TitleHeader();
            _year = DetailBelowHeader(_movie.releaseDate.Year.ToString(), xPadding);
            _runtime = DetailBelowHeader(_movie.runtime + " min", xPadding * 4);
            _genres = Genres(xPadding * 10);
            _description = DescriptionLabel();
            _imageView = Poster();
            _shadow = ShadowView();
            _line = CreateLine();


            this.View.AddSubviews(new UIView[] { _titleHeader, this._line, this._imageView, this._description, _runtime, _year, _genres});
        }

        // Helper functions
        private UILabel TitleHeader()
        {
            var promptLabel = new UILabel()
            {
                Frame = new CGRect(xPadding, 70, this.View.Bounds.Width - 10, headerTitleSize),
                Text = this._movie.title,
                AdjustsFontSizeToFitWidth = true,
                Font = UIFont.SystemFontOfSize(24),
            };

            return promptLabel;
        }

        private UILabel DetailBelowHeader(string text, double xLocation)
        {
            var promptLabel = new UILabel()
            {
                Frame = new CGRect(xLocation, 70 + 2 * yPadding, this.View.Bounds.Width - 10, headerTitleSize),
                Text = text,
                AdjustsFontSizeToFitWidth = true,
                Font = UIFont.SystemFontOfSize(10),
            };

            return promptLabel;
        }

        private UILabel Genres(double xLocation)
        {
            var genres = "";
            for (int i = 0; i < _movie.genres.Count; i++)
            {
                if(i + 1 == _movie.genres.Count)
                {
                    genres += _movie.genres[i];
                }
                else 
                {
                    genres += _movie.genres[i] + ", ";
                }
            }

            var text = fixGenreString(genres);

            var promptLabel = new UILabel()
            {
                Frame = new CGRect(xLocation, 70 + 2 * yPadding, this.View.Bounds.Width - 10, headerTitleSize),
                Text = text,
                AdjustsFontSizeToFitWidth = true,
                Font = UIFont.SystemFontOfSize(10),
            };

            return promptLabel;
        }

        private UILabel DescriptionLabel()
        {
            var promptLabel = new UILabel()
            {
                Frame = new CGRect(xPadding + ImageWidth + xPadding, topPadding + headerTitleSize + yPadding, this.View.Bounds.Width - xPadding - ImageWidth - 2 * xPadding, descriptionSize),
                Text = this._movie.description,
                Lines = 0,
                Font = UIFont.SystemFontOfSize(12)
            };

            return promptLabel;
        }

        private UIImageView Poster()
        {
            var imageView = new UIImageView()
            {
                Frame = new CGRect(xPadding, topPadding + headerTitleSize + 2 * yPadding , ImageWidth, ImageHeight),

            };

            imageView.Image = UIImage.FromFile(_movie.posterFilePath);

            return imageView;
        }
        private UIView ShadowView()
        {
            var shadowView = new UIView(this._imageView.Frame);
            shadowView.BackgroundColor = UIColor.White;
            shadowView.Layer.ShadowColor = UIColor.DarkGray.CGColor;
            shadowView.Layer.ShadowOpacity = 1.0f;
            shadowView.Layer.ShadowRadius = 6.0f;
            shadowView.Layer.ShadowOffset = new System.Drawing.SizeF(0f, 3f);
            shadowView.Layer.ShouldRasterize = true;
            shadowView.Layer.MasksToBounds = false;

            return shadowView;
        }

        private string fixGenreString(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ( (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ',')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private UIView CreateLine()
        {
            var frame = new CGRect(0, 70, this.View.Bounds.Width, 1);
            var line = new UIView(frame)
            {
                BackgroundColor = UIColor.Black
            };

            return line;
        }

        private async void GetMovieDetail(MovieDetails movie)
        {
            var detail = await _api.GetMovieDetail(movie.id);

            movie.budget = detail.budget;
            movie.description = detail.description;
            movie.genres = detail.genres;
            movie.tagLine = detail.tagLine;
            movie.runtime = detail.runtime;


            this._genres.Text = "";
            this._runtime.Text = "";
            ViewDidLoad();
        }



    }
}
