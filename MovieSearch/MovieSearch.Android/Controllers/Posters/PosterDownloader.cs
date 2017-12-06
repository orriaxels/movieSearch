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

namespace MovieSearch.Droid.Controllers.Posters
{

    public class PosterDownloader
    {
        private readonly string ImageUrl = "http://image.tmdb.org/t/p/original";
        private List<MovieDetails> _movieDetail;

        public PosterDownloader(List<MovieDetails> movieDetail)
        {
            _movieDetail = movieDetail;
        }


    }
}