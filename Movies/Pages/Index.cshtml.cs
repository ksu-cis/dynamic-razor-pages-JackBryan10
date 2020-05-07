using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Movies.Pages
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// The movies to display on the index page
        /// </summary>
        public IEnumerable<Movie> Movies { get; protected set; }

        /// <summary>
        /// The current search temrs
        /// </summary>
        [BindProperty]
        public string SearchTerms { get; set; }

        /// <summary>
        /// The filtered MPAA Ratings
        /// </summary>
        [BindProperty]
        public string[] MPAARatings { get; set; }

        /// <summary>
        /// The filtered Genres
        /// </summary>
        [BindProperty]
        public string[] Genres { get; set; }

        /// <summary>
        /// The minimum IMDB Rating
        /// </summary>
        [BindProperty]
        public double? IMDBMin { get; set; }

        /// <summary>
        /// The maximum IMDB Rating
        /// </summary>
        [BindProperty]
        public double? IMDBMax { get; set; }

        /// <summary>
        /// The minimum Rotten Tomatoes Rating
        /// </summary>
        [BindProperty]
        public double? RottenMin { get; set; }

        /// <summary>
        /// The maximum Rotten Tomatoes Rating
        /// </summary>
        [BindProperty]
        public double? RottenMax { get; set; }

        /// <summary>
        /// Gets the search results for display on the page
        /// </summary>
        public void OnGet()
        {
            Movies = MovieDatabase.All;
        }

        /// <summary>
        /// Posts the search results for display on the page
        /// </summary>
        /// <param name="IMDBMin">The minimum IMDB Rating</param>
        /// <param name="IMDBMax">The maximum IMDB Rating</param>
        /// <param name="RottenMin">The minimum Rotten Tomatoes Rating</param>
        /// <param name="RottenMax">The maximum Rotten Tomatoes Rating</param>
        public void OnPost()
        {
            /*
            Movies = MovieDatabase.Search(SearchTerms);
            Movies = MovieDatabase.FilterByMPAARating(Movies, MPAARatings);
            Movies = MovieDatabase.FilterByGenre(Movies, Genres);
            Movies = MovieDatabase.FilterByIMDBRating(Movies, IMDBMin, IMDBMax);
            Movies = MovieDatabase.FilterByRottenTomatoesRating(Movies, RottenMin, RottenMax);
            */

            Movies = MovieDatabase.All;

            // Search movie titles for the SearchTerms
            if (SearchTerms != null)
            {
                Movies = Movies.Where(movie =>
                    movie.Title != null &&
                    movie.Title.Contains(SearchTerms, StringComparison.CurrentCultureIgnoreCase)
                );
            }
            // Filter by MPAA Ratings
            if (MPAARatings != null && MPAARatings.Length != 0)
            {
                Movies = Movies.Where(movie =>
                    movie.MPAARating != null &&
                    MPAARatings.Contains(movie.MPAARating)
                );
            }
            // Filter by Genre 
            if (Genres != null && Genres.Count() != 0)
            {
                Movies = Movies.Where(movie =>
                    movie.MajorGenre != null &&
                    Genres.Contains(movie.MajorGenre)
                );
            }
            // Filter by IMBD Rating
            if (IMDBMax != null || IMDBMin != null)
            {
                // Min is null
                if (IMDBMin == null)
                {
                    Movies = Movies.Where(movie =>
                        movie.IMDBRating <= IMDBMax
                    );
                }
                // Max is null
                if (IMDBMax == null)
                {
                    Movies = Movies.Where(movie =>
                        movie.IMDBRating >= IMDBMin
                    );
                }
                // Both are specified
                if (IMDBMax != null && IMDBMin != null)
                {
                    Movies = Movies.Where(movie =>
                        movie.IMDBRating >= IMDBMin &&
                        movie.IMDBRating <= IMDBMax
                    );
                }
            }
            // Filter by RottenTomatoes Rating
            if (RottenMax != null || RottenMin != null)
            {
                // Min is null
                if (RottenMin == null)
                {
                    Movies = Movies.Where(movie =>
                        movie.RottenTomatoesRating <= RottenMax
                    );
                }
                // Max is null
                if (RottenMax == null)
                {
                    Movies = Movies.Where(movie =>
                        movie.RottenTomatoesRating >= RottenMin
                    );
                }
                // Both are specified
                if (RottenMax != null && RottenMin != null)
                {
                    Movies = Movies.Where(movie =>
                        movie.RottenTomatoesRating >= RottenMin &&
                        movie.RottenTomatoesRating <= RottenMax
                    );
                }
            }
        }
    }
}
