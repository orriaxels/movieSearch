using Android.OS;
using Android.Views;
using Fragment = Android.Support.V4.App.Fragment;
using MovieSearch.Services;
using MovieSearch.Droid.Controllers.Spinner;
using Android.Support.V4.App;
using Android.Widget;

namespace MovieSearch.Droid.Activities
{
    public class FavoriteFragment : Fragment
    {
        private MovieService _movieService;
        private SpinnerLoader _spinner;

        public FavoriteFragment(MovieService movieService)
        {
            this._movieService = movieService;
        }

        //public override void OnCreate(Bundle savedInstanceState)
        //{
        //    // Create your fragment here

        //    base.OnCreate(savedInstanceState);
        //}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var rootView = inflater.Inflate(Resource.Layout.Favorite, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);

            return rootView;
        }

        public override void OnStart()
        {
            base.OnStart();

            this._spinner = new SpinnerLoader(this.Context);
            this._spinner.setSpinnerMessage("Searching for Favorite Movies");
            this._spinner.show();
        }
    }
}
