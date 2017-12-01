using System;
using System.Diagnostics;
using System.Collections.Generic;
using DM.MovieApi;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.ApiResponse;
using System.Threading.Tasks;
using MovieSearch.Model;

namespace MovieSearch.Services
{
    public class MovieService
    {
        public string apiKey = "807f816252d83b681cf3b2efe5ffe5b0";
        public string apiUrl = "https://api.themoviedb.org/3/";

        private IApiMovieRequest _api;
        private List<MovieDetails> _movies;

        public MovieService()
        {
            MovieDbFactory.RegisterSettings(apiKey, apiUrl);
            _api = MovieDbFactory.Create<IApiMovieRequest>().Value;
            _movies = new List<MovieDetails>();
        }

        public async Task<List<MovieDetails>> GetMovieByTitle(string title)
        {
            ApiSearchResponse<MovieInfo> response = await _api.SearchByTitleAsync(title);

            if(_movies == null)
            {
                _movies.Clear();
            }
            else
            {
                _movies = new List<MovieDetails>();    
            }

            foreach (MovieInfo info in response.Results)
            {
                _movies.Add(new MovieDetails
                {
                    id = info.Id,
                    title = info.Title,
                    imageUrl = info.PosterPath,
                    releaseDate = info.ReleaseDate,
                    voteAverage = info.VoteAverage,
                    voteCount = info.VoteCount,
                    posterFilePath = "",
                    runtime = "",
                    genres = new List<String>(),
                    actors = new List<String>()
                });
            }

            return _movies;
        }

        public async Task<List<String>> GetCreditList(int id)
        {
            ApiQueryResponse<MovieCredit> cast = await _api.GetCreditsAsync(id);

            List<String> actors = new List<String>();

            for (int i = 0; i < cast.Item.CastMembers.Count && i < 3; i++)
            {
                actors.Add(cast.Item.CastMembers[i].Name);
            }    

            return actors;
        }

        public async Task<MovieDetails> GetMovieDetail(int id)
        {
            ApiQueryResponse<Movie> movieInfo = await _api.FindByIdAsync(id);

            MovieDetails movie = new MovieDetails()
            {
                title = movieInfo.Item.Title,
                runtime = movieInfo.Item.Runtime.ToString(),
                description = movieInfo.Item.Overview,
                tagLine = movieInfo.Item.Tagline,
                budget = movieInfo.Item.Budget,
                genres = new List<String>()
            };

            for (int i = 0; i < movieInfo.Item.Genres.Count; i++)
            {
                movie.genres.Add(movieInfo.Item.Genres[i].ToString());
            }

            return movie;
        }

        public List<MovieDetails> GetMovies()
        {
            return _movies;
        }

    }
}
