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
using Newtonsoft.Json;

namespace MovieSearch.Droid.Activities
{
	[Activity (Label = "MovieSearch.Android")]
	public class MainActivity : Activity
    {        
        public static MovieService MovieService { get; set; }
           
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);                  

            // Get our button from the layout resource,
            // and attach an event to it
            var movieSearchText = this.FindViewById<EditText>(Resource.Id.movieTitleInputField);
            var movieSearchbutton = FindViewById<Button> (Resource.Id.getMovieButton);

            movieSearchbutton.Click += async (object sender, EventArgs e) => 
            {
                ProgressDialog progressbar = new ProgressDialog(this);
                progressbar.SetCancelable(true);
                progressbar.SetMessage("Searching for movies...");
                progressbar.SetProgressStyle(ProgressDialogStyle.Spinner);
                progressbar.Show();
           
                var manager = (InputMethodManager)this.GetSystemService(InputMethodService);
                manager.HideSoftInputFromWindow(movieSearchText.WindowToken, 0);                
                await MovieService.GetMovieByTitle(movieSearchText.Text);                
                var intent = new Intent(this, typeof(MovieListActivity));
                intent.PutExtra("movieList", JsonConvert.SerializeObject(MovieService.GetMovies()));
                this.StartActivity(intent);
                progressbar.SetMessage("Finished!");
                progressbar.Hide();
            };
		}
	}
}


