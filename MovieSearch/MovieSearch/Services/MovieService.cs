using System;
using System.Linq;
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

            _movies = new List<MovieDetails>();

            if(response.Results == null)
            {
                return _movies;
            }

            if(response.Results != null)
            {
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
                        director = "",
                        genres = new List<String>(),
                        actors = new List<String>(),
                        characters = new List<String>()
                    });
                }
            }

            return _movies;
        }

        public async Task GetCreditList(MovieDetails movie)
        {
            ApiQueryResponse<MovieCredit> cast = await _api.GetCreditsAsync(movie.id);

            if(cast.Item != null)
            {
                for (int i = 0; i < cast.Item.CastMembers.Count && i < 3; i++)
                {
                    movie.actors.Add(cast.Item.CastMembers[i].Name);
                    movie.characters.Add(cast.Item.CastMembers[i].Character);
                }
            }

            if(cast.Item != null)
            {
                var director = cast.Item.CrewMembers.Single(x => x.Job == "Director");
                movie.director = director.Name;
            }
        }

        public async Task<MovieDetails> GetMovieDetail(int id)
        {
            ApiQueryResponse<Movie> movieInfo = await _api.FindByIdAsync(id);
            MovieDetails movie = new MovieDetails();

            if(movieInfo.Item != null)
            {
                movie = new MovieDetails()
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
            }

            return movie;
        }

        public async Task<List<MovieDetails>> getTopRatedMovies()
        {
            ApiSearchResponse<MovieInfo> response = await _api.GetTopRatedAsync();
            List<MovieDetails> movies = new List<MovieDetails>();

            Debug.WriteLine("Inside get top rated");

            if(response.Results == null)
            {
                return movies;
            }

            if (response.Results != null)
            {
                foreach (MovieInfo info in response.Results)
                {
                    movies.Add(new MovieDetails
                    {
                        id = info.Id,
                        title = info.Title,
                        imageUrl = info.PosterPath,
                        releaseDate = info.ReleaseDate,
                        voteAverage = info.VoteAverage,
                        voteCount = info.VoteCount,
                        posterFilePath = "",
                        runtime = "",
                        director = "",
                        genres = new List<String>(),
                        actors = new List<String>(),
                        characters = new List<String>()
                    });
                }
            }

            return movies;
        }

        public List<MovieDetails> GetMovies()
        {
            return _movies;
        }

    }
}
