using System.Linq;

namespace ToolBoxPG.DAL.Repository
{
    public interface IRepository<TEntity, TRetour> 
        where TEntity : class
        where TRetour : class
    {
        IQueryable<TRetour> GetAll();
        TRetour GetOne(params object[] Id);
        void Insert(TEntity Entity);
        void Update(TEntity Entity);
        void Delete(params object[] Id);
    }
}
