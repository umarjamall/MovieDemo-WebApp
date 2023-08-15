namespace MovieDemo.Models.Repository
{
    public class GenreRepo : IMovieDemoRepo<Genre>
    {
        AppDBContext dbContext;
        public GenreRepo(AppDBContext appDB)
        {
            dbContext = appDB;
        }


        public void Add(Genre entity)
        {
            dbContext.Add(entity);
            Save();
        }

        public void Delete(int? id)
        {
            var genre = Get(id);
            if (genre != null)
            {
                dbContext.Remove(genre);
            }
            Save();
        }

        public Genre Get(int? id)
        {
            var genre = dbContext.Genres.SingleOrDefault(x => x.GenreID == id);
            return genre;
        }

        public IList<Genre> GetAll()
        {
            return dbContext.Genres.ToList();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void Update(int id, Genre entity)
        {
            var genre = Get(id);
            if (genre != null)
            {
                dbContext.Update(genre);
            }
            Save();
        }
    }
}
