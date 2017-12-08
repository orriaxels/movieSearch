using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views.InputMethods;
using Newtonsoft.Json;
using Fragment = Android.Support.V4.App.Fragment;
using MovieSearch.Services;
using MovieSearch.Droid.Controllers.Spinner;
using MovieSearch.Droid.Activities;

namespace MovieSearch.Droid.Activities
{
    [Activity (Label = "Movie search", Theme = "@style/MyTheme", Icon = "@drawable/popcorn")]
    public class MainActivity : FragmentActivity
    {        
        public static MovieService MovieService { get; set; }
           
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            var movieSearchFrag = new MovieSearchFragment(MovieService);
            var favoriteFrag = new FavoriteFragment(MovieService);

            var fragments = new Fragment[]
            {
                movieSearchFrag,
                favoriteFrag
            };

            var titles = CharSequence.ArrayFromStringArray(new[] { "Search", "Favorite" });
            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);

            viewPager.Adapter = new TabsFragmentPagerAdapter(SupportFragmentManager, fragments, titles);

            var tabLayout = this.FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);

            tabLayout.TabSelected += async (sender, args) =>
            {
                if(args.Tab.Position == 1)
                {
                    await favoriteFrag.GetFavoriteMovies();
                }
                //if(args.Tab.Position == 0)
                //{
                //    favoriteFrag.ClearFavoriteMovieList();
                //}
            };

            var toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            this.SetActionBar(toolbar);
            this.ActionBar.Title = "Movie App";
		}
	}
}


