using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using MovieSearch.Model;
using MovieSearch.iOS.Views;

namespace MovieSearch.iOS.Controllers
{
    public class MovieListDataSource : UITableViewSource
    {
        private readonly List<MovieDetails> _movieList;
        public readonly NSString MovieListCellId = new NSString("MovieListCell");
        public readonly Action<int> _onSelectedMovie;

        public MovieListDataSource(List<MovieDetails> movieList, Action<int> onSelectedPerson)
        {
            this._movieList = movieList;
            this._onSelectedMovie = onSelectedPerson;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            //var cell = tableView.DequeueReusableCell((NSString)this.MovieListCellId);
            //if (cell == null)
            //{
            //    cell = new UITableViewCell(UITableViewCellStyle.Default, this.MovieListCellId);
            //}
            //cell.TextLabel.Text = this._movieList[indexPath.Row].title;
            //UIImage poster  = FromUrl(imagePath + this._movieList[indexPath.Row].imageUrl);
            //cell.ImageView.Image = poster;

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
            tableView.DeselectRow(indexPath, true);
            this._onSelectedMovie(indexPath.Row);
        }
    }
}
