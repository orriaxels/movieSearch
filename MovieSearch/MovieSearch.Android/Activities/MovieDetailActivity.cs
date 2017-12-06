﻿using System;
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

            var releaseYear = this.FindViewById<TextView>(Resource.Id.releaseYear);
            var genres = this.FindViewById<TextView>(Resource.Id.movieGenres);
            var runtime = this.FindViewById<TextView>(Resource.Id.runtime);
            var tagline = this.FindViewById<TextView>(Resource.Id.movieTagline);

            releaseYear.Text = _movie.releaseDate.Year.ToString();
            runtime.Text = _movie.runtime + " minutes";
            tagline.Text = _movie.description;

            var movieGenres = "";
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