using System;
using System.Collections.Generic;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;
using MovieSearch.Model;

namespace MovieSearch.iOS.Views
{
    public class MovieCell : UITableViewCell
    {
        private UIImageView _imageView;
        private const double ImageHeight = 96;
        private const double ImageWidth = 64;
        private UILabel _headingLabel;
        private UILabel _subheadingLabel;
        private UILabel _yearLabel;

        public MovieCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
        {
            this.SelectionStyle = UITableViewCellSelectionStyle.Gray;

            this._imageView = new UIImageView()
            {
                Frame = new CGRect(5, 5, ImageWidth, ImageHeight),
            };

            this._headingLabel = new UILabel()
            {
                Frame = new CGRect(75, 5, this.ContentView.Bounds.Width - 20, 25),
                Font = UIFont.SystemFontOfSize(16),
                TextColor = UIColor.FromRGB(0, 0, 0),
                BackgroundColor = UIColor.Clear
            };

            this._yearLabel = new UILabel()
            {
                Frame = new CGRect(75, 23, this.ContentView.Bounds.Width - 20, 25),
                Font = UIFont.SystemFontOfSize(15),
                TextColor = UIColor.FromRGB(0, 0, 0),
                BackgroundColor = UIColor.Clear
            };

            this._subheadingLabel = new UILabel()
            {
                Frame = new CGRect(75, 48, this.ContentView.Bounds.Width - 20, 20),
                Font = UIFont.SystemFontOfSize(12),
                TextColor = UIColor.FromRGB(145, 149, 161),
                BackgroundColor = UIColor.Clear
            };

            this.ContentView.AddSubviews(new UIView[] { this._imageView, this._headingLabel, this._yearLabel, this._subheadingLabel });

            this.Accessory = UITableViewCellAccessory.DisclosureIndicator;
        }

        //public void UpdateCell(string title, string releaseDate, List<string> actors, string imageName)
        public void UpdateCell(MovieDetails movie)
        {
            this._imageView.Image = UIImage.FromFile(movie.posterFilePath);
            this._headingLabel.Text = movie.title;
            this._yearLabel.Text = "(" + movie.ReleaseDate.Year + ")";
            var castMembers = "";

            for (int i = 0; i < movie.actors.Count; i++)
            {
                if(i == movie.actors.Count - 1)
                {
                    castMembers += movie.actors[i];
                }
                else
                {
                    castMembers += movie.actors[i] + ", ";
                }
            }

            if(movie.actors.Count == 0)
            {
                castMembers = "No cast members in db";
            }

            this._subheadingLabel.Text = castMembers;
        }
    }
}
