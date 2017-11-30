using System;
using System.Collections.Generic;
using DM.MovieApi.MovieDb.Movies;

namespace MovieSearch.Model
{
    public class MovieDetails
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string imageUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<string> actors { get; set; }
        public string posterFilePath { get; set; }
    }
}
