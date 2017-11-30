using System;
using System.Diagnostics;
using System.Collections.Generic;
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
            ApiSearchResponse<MovieInfo> response = await _api.SearchByTitleAsync(title);

            List<MovieDetails> moviesResults = new List<MovieDetails>();


            foreach (MovieInfo info in response.Results)
            {
                List<string> _actors = new List<string>();
                ApiQueryResponse<MovieCredit> cast = await _api.GetCreditsAsync(info.Id);
                MovieCredit actor = cast.Item;

                //if(actor.CastMembers.Count > 0)
                //{
                //    for (int i = 0; i < 3; i++)
                //    {
                //        if(actor.CastMembers[i] != null)
                //        {
                //            _actors.Add(actor.CastMembers[i].Name);    
                //        }
                //    }   
                //}

                if(actor.CastMembers.Count > 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        _actors.Add(actor.CastMembers[i].Name);
                    }    
                }

                MovieDetails newMovie = new MovieDetails()
                {
                    Id = info.Id,
                    title = info.Title,
                    imageUrl = info.PosterPath,
                    ReleaseDate = info.ReleaseDate,
                    actors = _actors
                };

                moviesResults.Add(newMovie);
            }

            return moviesResults;
        }
    }
}
