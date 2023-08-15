using Microsoft.EntityFrameworkCore;

namespace MovieDemo.Models.Repository
{
    public class MovieRepo : IMovieDemoRepo<Movie>
    {
        AppDBContext dbContext;
        public MovieRepo(AppDBContext appDB)
        {
            dbContext = appDB;
        }


        public void Add(Movie entity)
        {
            dbContext.Add(entity);
            Save();
        }

        public void Delete(int? id)
        {
            var mov = Get(id);
            if (mov != null)
            {
                dbContext.Remove(mov);
            }
            Save();
        }

        public Movie Get(int? id)
        {
            var mov = dbContext.Movies.Include(m => m.Genre).SingleOrDefault(x => x.MovieID == id);
            return mov;
        }

        public IList<Movie> GetAll()
        {
            return dbContext.Movies.ToList();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void Update(int id, Movie entity)
        {
            var mov = Get(id);
            if (mov != null)
            {
                dbContext.Update(mov);
            }
            Save();
        }
    }
}
