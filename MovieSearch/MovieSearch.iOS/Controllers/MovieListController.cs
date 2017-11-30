using System;
using System.Collections.Generic;
using System.Text;
using MovieSearch.Model;
using UIKit;

namespace MovieSearch.iOS.Controllers
{
    public class MovieListController : UITableViewController
    {
        private readonly List<MovieDetails> _movieList;

        public MovieListController(List<MovieDetails> movieList)
        {
            this._movieList = movieList;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.Title = "Movie list";

            this.TableView.Source = new MovieListDataSource(this._movieList, _onSelectedPerson);
        }

        private void _onSelectedPerson(int row)
        {
            var okAlertController = UIAlertController.Create("Movie selected", this._movieList[row].title,
                UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            this.PresentViewController(okAlertController, true, null);
        }
    }
}
