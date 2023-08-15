namespace MovieDemo.Models.Repository
{
    public interface IMovieDemoRepo<TEntity>
    {
        IList<TEntity> GetAll();
        TEntity Get(int? id);
        void Add(TEntity entity);
        void Update(int id,TEntity entity);
        void Delete(int? id);
        void Save();
    }
}
