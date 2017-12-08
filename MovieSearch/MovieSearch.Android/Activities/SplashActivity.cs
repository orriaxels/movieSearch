using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MovieSearch.Services;

namespace MovieSearch.Droid.Activities
{
    [Activity(Label = "SplashActivity", MainLauncher = true, Icon = "@drawable/popcorn", Theme = "@style/MyTheme.Splash")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //SetContentView(Resource.Layout.Splash);

            //ImageView image = FindViewById<ImageView>(Resource.Id.imageView1);
            //image.SetImageResource(Resource.Drawable.movieIcon);

            MainActivity.MovieService = new MovieService();

            System.Threading.Tasks.Task.Run(() => {
                Thread.Sleep(2000);
                StartActivity(typeof(MainActivity));
                this.Finish();
            });
            //this.StartActivity(typeof(MainActivity));
            //this.Finish();
            // Create your application here
        }
    }
}