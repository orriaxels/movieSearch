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
            var actors = this.FindViewById<TextView>(Resource.Id.actors);
            var characters = this.FindViewById<TextView>(Resource.Id.characters);
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

            for (int i = 0; i < persons.Count; i++)
            {
                var profileImage = (ImageView)this.FindViewById<ImageView>(Resource.Id.profile + i);
                if (persons[i].posterPath != "" || persons[i].posterPath != null)
                {
                    Glide.With(this).Load(ImageUrl + persons[i].posterPath).Into(profileImage);
                }                
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

            var allActors = "";
            var allCharacters = "";
            if (_movie.actors != null)
            {
                for (int i = 0; i < _movie.actors.Count; i++)
                {
                    if (i == _movie.actors.Count - 1)
                    {
                        allActors += _movie.actors[i];
                        allCharacters += "- " +_movie.characters[i];
                    }
                    else
                    {
                        allActors += _movie.actors[i] + "\n";
                        allCharacters += "- " +_movie.characters[i] + "\n";
                    }
                }
            }
            actors.Text = allActors;
            characters.Text = allCharacters;
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