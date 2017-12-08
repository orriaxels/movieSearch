using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using Newtonsoft.Json;
using Fragment = Android.Support.V4.App.Fragment;
using MovieSearch.Droid.Controllers.Spinner;
using MovieSearch.Services;
using System;
using Android.InputMethodServices;

namespace MovieSearch.Droid.Activities
{
    [Activity(Label = "Movie search",Theme = "@style/MyTheme")]
    public class MovieSearchFragment : Fragment
    {
        private MovieService _movieService;

        private SpinnerLoader _spinner;

        public MovieSearchFragment(MovieService movieService)
        {
            this._movieService = movieService;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            var rootView = inflater.Inflate(Resource.Layout.MovieSearch, container, false);

            // Get our button from the layout resource,
            // and attach an event to it
            var movieSearchText = rootView.FindViewById<EditText>(Resource.Id.movieTitleInputField);
            var movieSearchbutton = rootView.FindViewById<Button>(Resource.Id.getMovieButton);

            movieSearchText.Text = "Terminator";

            movieSearchbutton.Click += async (object sender, EventArgs e) =>
            {
                _spinner = new SpinnerLoader(this.Context);
                this._spinner.setSpinnerMessage("Searching for \"" + movieSearchText.Text + "\"");
                this._spinner.show();

                var manager = (InputMethodManager)this.Context.GetSystemService(Context.InputMethodService);
                manager.HideSoftInputFromWindow(movieSearchText.WindowToken, 0);
                await _movieService.GetMovieByTitle(movieSearchText.Text);
                var intent = new Intent(this.Context, typeof(MovieListActivity));
                intent.PutExtra("movieList", JsonConvert.SerializeObject(_movieService.GetMovies()));
                intent.PutExtra("searchText", "Results for \"" + movieSearchText.Text + "\"");

                this._spinner.setSpinnerMessage("Enjoy!");
                this._spinner.hide();

                this.StartActivity(intent);
            };

            return rootView;
        }
    }
}



