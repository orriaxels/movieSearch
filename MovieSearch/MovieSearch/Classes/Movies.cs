using System;
using System.Collections.Generic;
using MovieSearch.Model;
namespace MovieSearch.Classes
{
    public class Movies
    {
        private List<MovieDetails> _movies;

        public Movies(List<MovieDetails> movies)
        {
            this._movies = movies;
        }

        public List<MovieDetails> movies => this._movies;
    }
}
