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
using Android.Content.Res;
using static Android.Resource;
using Android.Support.V4.Content;

namespace MovieSearch.Droid.Activities
{
    [Activity(Label = "")]
    public class MovieDetailActivity : Activity
    {
        private readonly string ImageUrl = "http://image.tmdb.org/t/p/original";

        private MovieDetails _movie;
        private List<Person> persons;
        private ImageView _imageView;
        private MovieService _api;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var jsonString = this.Intent.GetStringExtra("movieDetail");
            this._movie = JsonConvert.DeserializeObject<MovieDetails>(jsonString);

            SetContentView(Resource.Layout.MovieDetail);
            _api = new MovieService();
            persons = _movie.person;

            var rating = _movie.voteAverage * 10;
            var releaseYear = this.FindViewById<TextView>(Resource.Id.releaseYear);
            var genres = this.FindViewById<TextView>(Resource.Id.movieGenres);
            var runtime = this.FindViewById<TextView>(Resource.Id.runtime);
            var director = this.FindViewById<TextView>(Resource.Id.director);
            var description = this.FindViewById<TextView>(Resource.Id.description);
            var writers = this.FindViewById<TextView>(Resource.Id.writer);
            var movieRating = this.FindViewById<TextView>(Resource.Id.movieRating);
            this.FindViewById<RatingBar>(Resource.Id.ratings).Progress = (int)rating;
            this._imageView = (ImageView)this.FindViewById<ImageView>(Resource.Id.moviePoster);
            
            movieRating.Text = _movie.voteAverage.ToString();         
            releaseYear.Text = _movie.releaseDate.Year.ToString();
            runtime.Text = _movie.runtime + " minutes";
            director.Text = "" + _movie.director;
            description.Text = _movie.description;

            if (_movie.imageUrl != "" || _movie.imageUrl != null)
            {
                Glide.With(this).Load(ImageUrl + _movie.imageUrl).Into(_imageView);
            }
            var color = new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.primary_material_light));


            for (int i = 0; i < persons.Count && i < 7; i++)
            {

                RelativeLayout layoutBase = FindViewById<RelativeLayout>(Resource.Id.rl3);
                var imageView = new ImageView(this) { Id = 7000 + i };                               
                var param = new RelativeLayout.LayoutParams(195, 390);               
                param.SetMargins(5, 0, 5, 0);
                if (i > 0)
                    param.AddRule(LayoutRules.RightOf, imageView.Id - 1);

                if (persons[i].posterPath != "" || persons[i].posterPath != null)
                {
                    Glide.With(this).Load(ImageUrl + persons[i].posterPath).Into(imageView);
                }
                layoutBase.AddView(imageView, param);


                // Getting the actor name
                var actorName = new TextView(this) { Id = 8000 + i, Text = _movie.actors[i], TextSize = 10 };
                actorName.SetMaxWidth(195);
                actorName.SetSingleLine(true);
                actorName.SetTextColor(color);
                actorName.SetPadding(8, 0, 8, 0);

                param = new RelativeLayout.LayoutParams(195, ViewGroup.LayoutParams.WrapContent);
                param.SetMargins(5, 0, 5, 0);                
                param.AddRule(LayoutRules.Below, imageView.Id);
                if (i > 0)
                {
                    param.AddRule(LayoutRules.RightOf, imageView.Id - 1);
                }
                layoutBase.AddView(actorName, param);


                // Getting the character name
                var characterName = new TextView(this) { Id = 9000 + i, Text = _movie.characters[i], TextSize = 8 };
                characterName.SetMaxWidth(195);
                characterName.SetSingleLine(true);
                characterName.SetPadding(8, 0, 8, 0);

                param = new RelativeLayout.LayoutParams(195, ViewGroup.LayoutParams.WrapContent);
                param.SetMargins(5, 0, 5, 0);
                param.AddRule(LayoutRules.Below, actorName.Id);
                if (i > 0)
                {
                    param.AddRule(LayoutRules.RightOf, imageView.Id - 1);
                }
                layoutBase.AddView(characterName, param);
            }

            var allWriters = "";
            for(int i = 0; i < _movie.writers.Count; i++)
            {
                if (i + 1 == _movie.writers.Count)
                {
                    allWriters += _movie.writers[i];
                }
                else
                {
                    allWriters += _movie.writers[i] + ", ";
                }
            }
            writers.Text = allWriters;

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

            //var allActors = "";
            //var allCharacters = "";
            //if (_movie.actors != null)
            //{
            //    for (int i = 0; i < _movie.actors.Count; i++)
            //    {
            //        if (i == _movie.actors.Count - 1)
            //        {
            //            allActors += _movie.actors[i];
            //            allCharacters += "- " +_movie.characters[i];
            //        }
            //        else
            //        {
            //            allActors += _movie.actors[i] + "\n";
            //            allCharacters += "- " +_movie.characters[i] + "\n";
            //        }
            //    }
            //}
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