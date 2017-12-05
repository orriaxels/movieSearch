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

namespace MovieSearch.Droid.Activities
{
    public class MovieListAdapter : BaseAdapter<MovieDetails>
    {

        private readonly Activity _context;
        private readonly List<MovieDetails> _movieDetails;
        private readonly MovieService _api;
        private ImageDownloader _imageDownloader;
        IImageStorage storageClient = new StorageClient();

        public MovieListAdapter(Activity context, List<MovieDetails> movieDetails, MovieService api)
        {
            this._context = context;
            this._movieDetails = movieDetails;
            this._api = api;
            this._imageDownloader = new ImageDownloader(storageClient);
            GetAllCastMembers(_movieDetails);
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

            if (view == null)
            {
                view = this._context.LayoutInflater.Inflate(Resource.Layout.MovieListItem, null);
            }
                
            var movie = this._movieDetails[position];
            
            view.FindViewById<TextView>(Resource.Id.title).Text = movie.title;
            view.FindViewById<TextView>(Resource.Id.year).Text = "(" + movie.releaseDate.Year.ToString() + ")";
            var resourceId = this._context.Resources.GetIdentifier(movie.imageUrl, "drawable", this._context.PackageName);
            view.FindViewById<ImageView>(Resource.Id.poster).SetBackgroundResource(resourceId);

            var allActors = "";

            if (movie.actors != null)
            {
                for (int i = 0; i < movie.actors.Count; i++)
                {
                    if (i == movie.actors.Count - 1)
                    {
                        allActors += movie.actors[i];
                    }
                    else
                    {
                        allActors += movie.actors[i] + ", ";
                    }
                }
            }

            view.FindViewById<TextView>(Resource.Id.actors).Text = allActors;

            return view;
        }

        private async void DownloadPosters(List<MovieDetails> movies)
        {
            await _imageDownloader.DownloadImagesInList(movies);
        }

        private async void GetAllCastMembers(List<MovieDetails> movies)
        {
            foreach (MovieDetails movie in movies)
            {
                await _api.GetCreditList(movie);
            }
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