using Android.OS;
using Android.Views;
using Fragment = Android.Support.V4.App.Fragment;
using MovieSearch.Services;
using MovieSearch.Droid.Controllers.Spinner;
using Android.Support.V4.App;
using Android.Widget;
using System.Threading.Tasks;
using Android.Views.InputMethods;
using System.Collections.Generic;
using MovieSearch.Model;
using Android.Content;
using Newtonsoft.Json;

namespace MovieSearch.Droid.Activities
{
    public class FavoriteFragment : Fragment
    {
        private MovieService _api;
        private SpinnerLoader _spinner;
        private List<MovieDetails> _movieList;
        private ListView _listView;
        private MovieDetails _movie;

        public FavoriteFragment(MovieService movieService)
        {
            this._api = movieService;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            // Create your fragment here

            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var rootView = inflater.Inflate(Resource.Layout.Favorite, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            this._listView = rootView.FindViewById<ListView>(Resource.Id.listView1);

            this._listView.ItemClick += async (sender, args) =>
            {
                _movie = _movieList[args.Position];

                _spinner = new SpinnerLoader(this.Context);
                this._spinner.setSpinnerMessage("Getting details on \"" + _movie.title + "\"");
                this._spinner.show();

                await _api.GetCastMembers(_movie);
                var intent = new Intent(this.Activity, typeof(MovieDetailActivity));
                intent.PutExtra("movieDetail", JsonConvert.SerializeObject(_movie));

                this.StartActivity(intent);

                this._spinner.setSpinnerMessage("Enjoy!");
                this._spinner.hide();
            };

            return rootView;
        }

        public async Task GetFavoriteMovies()
        {
            this._movieList = new List<MovieDetails>();
            this._movieList.Clear();

            _spinner = new SpinnerLoader(this.Context);
            this._spinner.setSpinnerMessage("Getting favorite movies");
            this._spinner.show();

            this._movieList = await _api.getTopRatedMovies();
            this._listView.Adapter = new MovieListAdapter(this.Activity, this._movieList, this._api);
            this._spinner.setSpinnerMessage("Enjoy!");
            this._spinner.hide();
        }

        public void ClearFavoriteMovieList()
        {
            this._movieList.Clear();
            this._movieList = null;
        }
    }
}
