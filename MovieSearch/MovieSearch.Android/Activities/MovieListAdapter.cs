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
using MovieDownload;
using Com.Bumptech.Glide;

namespace MovieSearch.Droid.Activities
{
    public class MovieListAdapter : BaseAdapter<MovieDetails>
    {
        private readonly string ImageUrl = "http://image.tmdb.org/t/p/original";
        private readonly Activity _context;
        private readonly List<MovieDetails> _movieDetails;
        private readonly MovieService _api;
        private ImageView _imageView;

        public MovieListAdapter(Activity context, List<MovieDetails> movieDetails, MovieService api)
        {
            this._context = context;
            this._movieDetails = movieDetails;
            this._api = api;
            GetMovieDetail(_movieDetails);
            this.GetAllCastMembers(_movieDetails);

        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            var allActors = "";

            if (view == null)
            {
                view = this._context.LayoutInflater.Inflate(Resource.Layout.MovieListItem, null);
            }
                
            var movie = this._movieDetails[position];
            var rating = movie.voteAverage * 10;
            Console.WriteLine(rating);
            view.FindViewById<TextView>(Resource.Id.title).Text = movie.director;
            view.FindViewById<TextView>(Resource.Id.year).Text = "(" + movie.releaseDate.Year.ToString() + ")";
            view.FindViewById<RatingBar>(Resource.Id.ratings).Progress = (int)rating;
            this._imageView = (ImageView)view.FindViewById<ImageView>(Resource.Id.poster);

            if(movie.imageUrl != "" || movie.imageUrl != null)
            {
                Glide.With(this._context).Load(ImageUrl + movie.imageUrl).Into(_imageView);    
            }


            if (movie.actors != null)
            {
                for (int i = 0; i < movie.actors.Count; i++)
                {
                    if (i == movie.actors.Count - 1)
                    {
                        allActors += movie.actors[i] + " - " + movie.characters[i];
                    }
                    else
                    {
                        allActors += movie.actors[i] + " - " + movie.characters[i] + "\n";
                    }
                }
            }

            view.FindViewById<TextView>(Resource.Id.actors).Text = allActors;
            return view;
        }

        private async void GetAllCastMembers(List<MovieDetails> movies)
        {
            foreach (MovieDetails movie in movies)
            {
                await _api.GetCreditList(movie);
                NotifyDataSetChanged();
            }
        }

        private async void GetMovieDetail(List<MovieDetails> movie)
        {
            foreach (MovieDetails m in movie)
            {
                var detail = await _api.GetMovieDetail(m.id);

                m.budget = detail.budget;
                m.description = detail.description;
                m.genres = detail.genres;
                m.tagLine = detail.tagLine;
                m.runtime = detail.runtime;
            }

            NotifyDataSetChanged();

        }

        //Fill in cound here, currently 0
        public override int Count => this._movieDetails.Count;        

        public override MovieDetails this[int position] => throw new NotImplementedException();
    }

    class MovieListAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}