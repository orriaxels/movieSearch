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
using MovieSearch.Services;
using Newtonsoft.Json;

namespace MovieSearch.Droid.Activities
{
    [Activity(Label = "")]
    public class MovieDetailActivity : Activity
    {
        private MovieDetails _movie;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var jsonString = this.Intent.GetStringExtra("movieDetail");
            this._movie = JsonConvert.DeserializeObject<MovieDetails>(jsonString);

            SetContentView(Resource.Layout.MovieDetail);        
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            Window.SetTitle(_movie.title);
        }
    }
}