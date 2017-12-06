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
using Com.Bumptech.Glide;

namespace MovieSearch.Droid.Activities
{
    [Activity(Label = "")]
    public class MovieDetailActivity : Activity
    {
        private readonly string ImageUrl = "http://image.tmdb.org/t/p/original";

        private MovieDetails _movie;
        private ImageView _imageView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var jsonString = this.Intent.GetStringExtra("movieDetail");
            this._movie = JsonConvert.DeserializeObject<MovieDetails>(jsonString);

            
            SetContentView(Resource.Layout.MovieDetail);

            var releaseYear = this.FindViewById<TextView>(Resource.Id.releaseYear);
            var genres = this.FindViewById<TextView>(Resource.Id.movieGenres);
            var runtime = this.FindViewById<TextView>(Resource.Id.runtime);
            var tagline = this.FindViewById<TextView>(Resource.Id.movieTagline);
            this._imageView = (ImageView)this.FindViewById<ImageView>(Resource.Id.moviePoster);

            releaseYear.Text = _movie.releaseDate.Year.ToString();
            runtime.Text = _movie.runtime + " minutes";
            tagline.Text = _movie.description;

            if (_movie.imageUrl != "" || _movie.imageUrl != null)
            {
                Glide.With(this).Load(ImageUrl + _movie.imageUrl).Into(_imageView);
            }

            var movieGenres = "";
            if(_movie.genres != null)
            {
                for (int i = 0; i < _movie.genres.Count; i++)
                {
                    if (i + 1 == _movie.genres.Count)
                    {
                        movieGenres += _movie.genres[i];
                    }
                    else
                    {
                        movieGenres += _movie.genres[i] + ", ";
                    }
                }
                genres.Text = fixGenreString(movieGenres);
            }
            
        }

        private string fixGenreString(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
                else if(c == ',')
                {
                    sb.Append(c);
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            Window.SetTitle(_movie.title);
        }
    }
}