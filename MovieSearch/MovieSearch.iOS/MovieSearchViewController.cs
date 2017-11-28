using System;
using CoreGraphics;
using MovieSearch.Model;
using System.Linq;

using UIKit;

namespace MovieSearch.iOS
{
    public partial class MovieSearchViewController : UIViewController
    {
        private MyMovieApi _api;
        public MovieSearchViewController(MyMovieApi api) : base("MovieSearchViewController", null)
        {
            _api = api;
        }

        private const double StartX = 20;
        private const double StartY = 80;
        private const double Height = 50;
        public string resultTitle;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.BackgroundColor = UIColor.White;

            var promptLabel = new UILabel()
            {
                Frame = new CGRect(StartX, StartY, this.View.Bounds.Width - 2 * StartX, Height),
                Text = "Enter words in movie title:"
            };
            Console.WriteLine("Bounds: " + this.View.Bounds.Width);
            var titleField = new UITextField()
            {
                Frame = new CGRect(StartX, StartY + Height, 320, Height),
                BorderStyle = UITextBorderStyle.RoundedRect
            };

            var resultField = new UILabel()
            {
                Frame = new CGRect(StartX, StartY + 3 * Height, this.View.Bounds.Width - 2 * StartX, Height),
                Text = ""
            };

            var getMovieButton = UIButton.FromType(UIButtonType.RoundedRect);
            getMovieButton.Frame = new CGRect(StartX, StartY + 2 * Height, this.View.Bounds.Width / 2, Height);
            getMovieButton.SetTitle("Get movie", UIControlState.Normal);

            getMovieButton.TouchUpInside += async (sender, args) =>
            {
                titleField.ResignFirstResponder();
                resultTitle = titleField.Text;

                if (resultTitle != "")
                {
                    var result = await _api.GetMovieByTitle(resultTitle);

                    resultField.Text = result[0].title;
                }

            };

            this.View.AddSubview(promptLabel);
            this.View.AddSubview(titleField);
            this.View.AddSubview(getMovieButton);
            this.View.AddSubview(resultField);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

