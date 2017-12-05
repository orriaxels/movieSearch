using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MovieSearch.Model;
using System.Collections.Generic;
using MovieSearch.Services;
using Android.Views.InputMethods;
using System.Threading;
using MovieSearch.Droid.Controllers.Spinner;

namespace MovieSearch.Droid
{
	[Activity (Label = "MovieSearch.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
    {
        private List<MovieDetails> _movieList;
        private MovieService _api;
        private SpinnerLoader _spinner;

        public MainActivity()
        {
            _api = new MovieService();
            _movieList = new List<MovieDetails>();
        }
           
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);                  

            // Get our button from the layout resource,
            // and attach an event to it
            var movieSearchText = this.FindViewById<EditText>(Resource.Id.movieTitleInputField);
            var movieSearchbutton = FindViewById<Button> (Resource.Id.getMovieButton);
            var resultText = this.FindViewById<TextView>(Resource.Id.resultText);

            movieSearchbutton.Click += async (object sender, EventArgs e) => 
            {
                _spinner = new SpinnerLoader(this);
                this._spinner.setSpinnerMessage("Searching for movies...");
                this._spinner.show();

                resultText.Text = "";
                var manager = (InputMethodManager)this.GetSystemService(InputMethodService);
                manager.HideSoftInputFromWindow(movieSearchText.WindowToken, 0);
                _movieList = await _api.GetMovieByTitle(movieSearchText.Text);
                for(int i = 0; i < _movieList.Count; i++)
                {
                    resultText.Text += _movieList[i].title + "\n";
                }

                this._spinner.setSpinnerMessage("Finished!");
                this._spinner.hide();
            };
		}
	}
}


