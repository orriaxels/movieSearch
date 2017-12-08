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
using MovieSearch.Droid.Controllers.Spinner;

namespace MovieSearch.Droid.Activities
{
    [Activity (Label = "Movie search")]
    public class MovieListActivity : ListActivity
    {
        private List<MovieDetails> _movieList;
        private MovieDetails _movie;
        private MovieService _api = new MovieService();
        private String searchText;
        private SpinnerLoader _spinner;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            var jsonString = this.Intent.GetStringExtra("movieList");
            searchText = this.Intent.GetStringExtra("searchText");
            this._movieList = JsonConvert.DeserializeObject<List<MovieDetails>>(jsonString);

            this.ListView.ItemClick += async (sender, args) =>
            {
                _movie = _movieList[args.Position];
                _spinner = new SpinnerLoader(this);
                this._spinner.setSpinnerMessage("Getting details on \"" + _movie.title + "\"");
                this._spinner.show();
                
                await _api.GetCastMembers(_movie);
                var intent = new Intent(this, typeof(MovieDetailActivity));
                intent.PutExtra("movieDetail", JsonConvert.SerializeObject(_movie));

                this.StartActivity(intent);

                this._spinner.setSpinnerMessage("Enjoy!");
                this._spinner.hide();
            };

            this.ListAdapter = new MovieListAdapter(this, this._movieList, this._api);           
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            Window.SetTitle(searchText);
        }
    }
}