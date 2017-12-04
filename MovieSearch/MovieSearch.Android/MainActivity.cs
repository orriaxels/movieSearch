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

namespace MovieSearch.Droid
{
	[Activity (Label = "MovieSearch.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
    {
        private List<MovieDetails> _movieList;
        private MovieService _api;


        public MainActivity()
        {
            _api = new MovieService();
            _movieList = new List<MovieDetails>();
        }

        //public MainActivity(MovieService api)
        //{
        //    _api = api;
        //    _movieList = new List<MovieDetails>();
        //}
            
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
                var manager = (InputMethodManager)this.GetSystemService(InputMethodService);
                manager.HideSoftInputFromWindow(movieSearchText.WindowToken, 0);
                _movieList = await _api.GetMovieByTitle(movieSearchText.Text);
                resultText.Text = _movieList[0].title;
			};

            Console.WriteLine(_movieList);
		}
	}
}


