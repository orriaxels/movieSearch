﻿using System;
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
    [Activity (Label = "Movie search", Theme = "@style/MyTheme")]
    public class MainActivity : FragmentActivity
    {        
        public static MovieService MovieService { get; set; }
         
        //private SpinnerLoader _spinner;
           
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            var fragments = new Fragment[]
            {
                new MovieSearchFragment(MovieService),
                new FavoriteFragment(),
            };

            var titles = CharSequence.ArrayFromStringArray(new[] { "Search", "Favorite" });
            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);

            viewPager.Adapter = new TabsFragmentPagerAdapter(SupportFragmentManager, fragments, titles);

            var tabLayout = this.FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);

            var toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            this.SetActionBar(toolbar);
            this.ActionBar.Title = "Movie App";
		}
	}
}


