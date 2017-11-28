using System;
using System.Diagnostics;
using System.Collections.Generic;
using DM.MovieApi.ApiRequest;
using DM.MovieApi;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.ApiResponse;
using System.Threading.Tasks;
using MovieSearch.Model;


namespace MovieSearch
{
    public class MyMovieApi
    {
        public string apiKey = "807f816252d83b681cf3b2efe5ffe5b0";
        public string apiUrl = "https://api.themoviedb.org/3/";
        private List<string> movies;

        private IApiMovieRequest _api;

        public MyMovieApi()
        {
            MovieDbFactory.RegisterSettings(apiKey, apiUrl);
            _api = MovieDbFactory.Create<IApiMovieRequest>().Value;
            movies = new List<string>();
        }

        public async Task<List<MovieDetails>> GetMovieByTitle(string title)
        {
            Debug.WriteLine("inside api: " + title);

            ApiSearchResponse<MovieInfo> response = await _api.SearchByTitleAsync(title);

            //Debug.WriteLine("after await");

            List<MovieDetails> moviesResults = new List<MovieDetails>();

            foreach (MovieInfo info in response.Results)
            {
                MovieDetails newMovie = new MovieDetails()
                {
                    Id = info.Id,
                    title = info.Title
                };

                moviesResults.Add(newMovie);
            }

            return moviesResults;
        }
    }
}
