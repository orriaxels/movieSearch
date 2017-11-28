using System;
using DM.MovieApi;

namespace MovieSearch
{
    public class MovieDbSettings : IMovieDbSettings
    {
        public string ApiKey { get; }
        public string ApiUrl { get; }

        public MovieDbSettings(string apiKey, string apiUrl)
        {
            this.ApiKey = apiKey;
            this.ApiUrl = apiUrl;
        }
    }
}
