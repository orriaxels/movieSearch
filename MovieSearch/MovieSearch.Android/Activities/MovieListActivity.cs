using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MovieSearch.Model;
using Newtonsoft.Json;
using MovieSearch.Services;

namespace MovieSearch.Droid.Activities
{
    [Activity(Label = "MovieListActivity")]
    public class MovieListActivity : ListActivity
    {
        private List<MovieDetails> _movieList;
        private MovieService _api = new MovieService();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var jsonString = this.Intent.GetStringExtra("movieList");
            this._movieList = JsonConvert.DeserializeObject<List<MovieDetails>>(jsonString);

            this.ListView.ItemClick += (sender, args) =>
            {
                //var intent = new Intent(this, typeof(MovieDetailActivity));
                //intent.PutExtra("movieDetail", JsonConvert.SerializeObject())
            };

            this.ListAdapter = new MovieListAdapter(this, this._movieList, this._api);
        
            // Create your application here
        }

        //private void ShowAlert(int position)
        //{
        //    var movie = this._movieList[position];
        //    var alertBuilder = new AlertDialog.Builder(this);
        //    alertBuilder.SetTitle("Person selected");
        //    alertBuilder.SetMessage(movie.title);
        //    alertBuilder.SetMessage(movie.runtime);
        //    alertBuilder.SetCancelable(true);
        //    alertBuilder.SetPositiveButton("OK", (e, args) => { });
        //    var dialog = alertBuilder.Create();
        //    dialog.Show();
        //}
    }
}