using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using MovieSearch.Model;
using MovieSearch.iOS.Views;
using MovieSearch.iOS.Controllers;

namespace MovieSearch.iOS.Controllers
{
    public class MovieListDataSource : UITableViewSource
    {
        private readonly List<MovieDetails> _movieList;
        public readonly NSString MovieListCellId = new NSString("MovieListCell");
        public readonly Action<int> _onSelectedMovie;

        public MovieListDataSource(List<MovieDetails> movieList, Action<int> onSelectedMovie)
        {
            this._movieList = movieList;
            this._onSelectedMovie = onSelectedMovie;

        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (MovieCell)tableView.DequeueReusableCell((NSString)this.MovieListCellId);
            if (cell == null)
            {
                cell = new MovieCell(this.MovieListCellId); 
            }

            var movie = this._movieList[indexPath.Row];
            cell.UpdateCell(movie);
            GetHeightForRow(tableView, indexPath);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this._movieList.Count;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 106;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //NavigationItem.BackBarButtonItem = new UIBarButtonItem("Movie search", UIBarButtonItemStyle.Plain, null);
            //this.NavigationController.PushViewController(new MovieListController(this._movieList), true);


            tableView.DeselectRow(indexPath, true);
            //var movieDetail = this._movieList[indexPath.Row];
            this._onSelectedMovie(indexPath.Row);
        }
    }
}
