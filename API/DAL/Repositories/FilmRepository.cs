using DAL.Context;
using FilmsSpeedRunAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class FilmRepository : GenericRepository<Film>
    {
        public FilmRepository(FilmContext context) : base(context)
        {

        }
        public List<Film> Search(string title, int page = 0)
        {
            FilmContext con = (FilmContext)context;
            return con.Films.Where(x => x.Title.ToLower().Contains(title.ToLower()))
                            .Skip(page * 10)
                            .Take(10)
                            .ToList();
        }
        public List<Film> GetSortedFilter(FilterOptions options)
        {
            //also you need to think about non genre filter
            FilmContext con = (FilmContext)context;
            switch (options.SortBy.ToLower())
            {
                case "releasedate":
                    return con.Films
                                .OrderByDescending(x => x.DateOfPublishing)
                                .Where(x => x.ImdbRating >= options.ImdbRatingTop && x.ImdbRating <= options.ImdbRatingLast)
                                .Where(x => x.LocalRating >= options.LocalRatingTop && x.LocalRating <= options.LocalRatingLast)
                                .Where(x => x.DateOfPublishing >= options.DateTop && x.DateOfPublishing <= options.DateLast)
                                //.Where(x => x.Genres.Contains(con.Genres.FirstOrDefault(x => x.Name == options.Genre)))
                                .Skip(options.Page * 10)
                                .Take(10)
                                .ToList();
                case "localrating":
                    return SortAndFilter(options, x => x.LocalRating);
                case "imdbrating":
                    return SortAndFilter(options, x => x.ImdbRating);
                case "title":
                    return SortAndFilter(options, x => x.Title);
                default:
                    return con.Films
                                .Where(x => x.ImdbRating >= options.ImdbRatingTop && x.ImdbRating <= options.ImdbRatingTop)
                                .Where(x => x.LocalRating >= options.LocalRatingTop && x.LocalRating <= options.LocalRatingLast)
                                .Where(x => x.DateOfPublishing >= options.DateTop && x.DateOfPublishing <= options.DateLast)
                                .Where(x => x.Genres.Contains(con.Genres.FirstOrDefault(x => x.Name == options.Genre)))
                                .Skip((options.Page - 1) * 10)
                                .Take(10)
                                .ToList();
            }
        }
        List<Film> SortAndFilter(FilterOptions options, Expression<Func<Film, object>> selector)
        {
            FilmContext con = (FilmContext)context;
            return con.Films
                        .OrderByDescending(selector)
                        .Where(x => x.ImdbRating >= options.ImdbRatingTop && x.ImdbRating <= options.ImdbRatingTop)
                        .Where(x => x.LocalRating >= options.LocalRatingTop && x.LocalRating <= options.LocalRatingLast)
                        .Where(x => x.DateOfPublishing >= options.DateTop && x.DateOfPublishing <= options.DateLast)
                        .Where(x => x.Genres.Contains(con.Genres.FirstOrDefault(x => x.Name == options.Genre)))
                        .Skip(options.Page * 10)
                        .Take(10)
                        .ToList();
        }
        public List<Actor> GetActors(int filmId)
        {
            FilmContext contx = (FilmContext)context;
            var list = contx.Films.Include(x => x.Actors)
                                .ToList()
                       .Where(x => x.Id == filmId)
                       .Select(x => x.Actors)
                       .ToList()[0]
                       .ToList();
            foreach (Actor actor in list)
                actor.Films = null;
            return list;
        }
        public List<Writer> GetWriters(int filmId)
        {
            FilmContext contx = (FilmContext)context;
            var list = contx.Films.Include(x => x.Writers)
                                .ToList()
                       .Where(x => x.Id == filmId)
                       .Select(x => x.Writers)
                       .ToList()[0]
                       .ToList();
            foreach (Writer writer in list)
                writer.Films = null;
            return list;
        }
        public List<Genre> GetGenres(int filmId)
        {
            FilmContext contx = (FilmContext)context;
            var list = contx.Films.Include(x => x.Genres)
                                .ToList()
                       .Where(x => x.Id == filmId)
                       .Select(x => x.Genres)
                       .ToList()[0]
                       .ToList();
            foreach (Genre genre in list)
                genre.Films = null;
            return list;
        }
        public List<Director> GetDirectors(int filmId)
        {
            FilmContext contx = (FilmContext)context;
            var list = contx.Films.Include(x => x.Directors)
                                .ToList()
                      .Where(x => x.Id == filmId)
                      .Select(x => x.Directors)
                      .ToList()[0]
                      .ToList();
            foreach (Director director in list)
                director.Films = null;
            return list;
        }
        public List<Selection> GetSelections(int filmId)
        {
            FilmContext contx = (FilmContext)context;
            var list = contx.Films.Include(x => x.Selections)
                                .ToList()
                      .Where(x => x.Id == filmId)
                      .Select(x => x.Selections)
                      .ToList()[0]
                      .ToList();
            foreach (Selection selection in list)
                selection.Films = null;
            return list;
        }
        public Company GetCompany(int filmId)
        {
            FilmContext contx = (FilmContext)context;
            var company = contx.Films.Include(x => x.Company)
                                .ToList()
                        .Where(x => x.Id == filmId)
                        .Select(x => x.Company)
                        .ToList()[0];
            company.Films = null;
            return company;
        }
        public Film GetFull(Film film)
        {
            FilmContext con = (FilmContext)context;
            return con.Films.Include(x => x.Actors)
                             .Include(x => x.Writers)
                             .Include(x => x.Directors)
                             .Include(x => x.Genres)
                             .FirstOrDefault(x => x.ImdbId == film.ImdbId);
        }
        public List<Comment> GetComments(int filmId)
        {
            FilmContext con = (FilmContext)context;
            var list = con.Films.Include(x => x.Comments)
                            .FirstOrDefault(x => x.Id == filmId)
                            .Comments;
            list.ForEach(x => x.CommentedFilm = null);
            return list;
        }
    }
}
