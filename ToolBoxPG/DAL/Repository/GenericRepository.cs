using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace ToolBoxPG.DAL.Repository
{
    public abstract class GenericRepository<TEntity, TRetour> : IRepository<TEntity, TRetour>
        where TEntity : class
        where TRetour : class
    {
        protected GenericRepository(DbContext DataBase)
        {
            Context = DataBase;
            DbSet = Context.Set<TEntity>();
        }
        protected DbContext Context;
        protected DbSet<TEntity> DbSet;
        /// <summary>
        /// Delete l'entity avec l'id donner de maniere local tant que l'on a pas save
        /// </summary>
        /// <param name="Id">l'ensemble des champs qui compose la primary key</param>
        public virtual void Delete(params object[] Id)
        {
            TEntity EntityToDelete = DbSet.Find(Id);

            Delete(EntityToDelete);
        }
        /// <summary>
        /// Delete l'entity de maniere local tant que l'on a pas save
        /// </summary>
        /// <param name="EntityToDelete">l'entity à supprimer</param>
        public virtual void Delete(TEntity EntityToDelete)
        {
            if (Context.Entry(EntityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(EntityToDelete);
            }
            DbSet.Remove(EntityToDelete);
        }
        /// <summary>
        /// Delete l'entity dde maniere local tant que l'on a pas save
        /// </summary>
        /// <param name="ClientEntityToDelete">l'entity à supprimer</param>
        public virtual void Delete(TRetour ClientEntityToDelete)
        {
            TEntity EntityToDelete = RetourToEntity(ClientEntityToDelete);
            Delete(EntityToDelete);
        }
        /// <summary>
        /// fait un select * sur la table
        /// </summary>
        /// <returns>retour un IQueryable</returns>
        public virtual IQueryable<TRetour> GetAll()
        {
            List<TRetour> Retour = DbSet.Select(Item => EntityToRetour(Item)).ToList();

            IQueryable<TRetour> Query = Retour.AsQueryable();
            return Query;
        }

        /// <summary>
        /// fait un select sur la table
        /// </summary>
        /// <param name="Filter"> expression regulier qui sert a faire le where de la requet select</param>
        /// <param name="OrderBy"> du type "OrderBy: c => c.OrderBy(d => d.property))"</param>
        /// <returns>retour un IQueryable sur base de la close donner dans filter et ordonner suivant le orderby</returns>
        public virtual IQueryable<TRetour> GetAll(
            Expression<Func<TEntity, bool>> Filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy
            )
        {
            IQueryable<TEntity> Query = DbSet;

            if (Filter != null)
            {
                Query = Query.Where(Filter);//applique le filtre a l'ensemble des entity compris dans query.
            }

            if (OrderBy != null)
            {
                Query = OrderBy(Query);//ordonne les entity compris dan la querry suivant la fonction orderby passé en parametre.
            }

            List<TRetour> Retour = new List<TRetour>();

            foreach (TEntity Item in Query)
            {
                Retour.Add(EntityToRetour(Item));
            }

            return Retour.AsQueryable();
        }

        /// <summary>
        /// fait un select sur la table
        /// </summary>
        /// <param name="Filter"> expression regulier qui sert a faire le where de la requet select</param>
        /// <returns>retour un IQueryable sur base de la close donner dans filter</returns>
        public virtual IQueryable<TRetour> GetAll(
            Expression<Func<TEntity, bool>> Filter
            )
        {
            IQueryable<TEntity> Query = DbSet;

            if (Filter != null)
            {
                Query = Query.Where(Filter);//applique le filtre a l'ensemble des entity compris dans query.
            }

            List<TRetour> Retour = new List<TRetour>();

            foreach (TEntity Item in Query)
            {
                Retour.Add(EntityToRetour(Item));
            }

            return Retour.AsQueryable();
        }

        /// <summary>
        /// fait un select sur la table
        /// </summary>
        /// <param name="OrderBy"> du type "OrderBy: c => c.OrderBy(d => d.property))"</param>
        /// <returns>retour un IQueryable ordonner suivant le orderby</returns>
        public virtual IQueryable<TRetour> GetAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy
            )
        {
            IQueryable<TEntity> Query = DbSet;

            if (OrderBy != null)
            {
                Query = OrderBy(Query);//ordonne les entity compris dan la querry suivant la fonction orderby passé en parametre.
            }

            List<TRetour> Retour = new List<TRetour>();

            foreach (TEntity Item in Query)
            {
                Retour.Add(EntityToRetour(Item));
            }

            return Retour.AsQueryable();
        }

        /// <summary>
        /// fait un select sur la table
        /// </summary>
        /// <param name="Filter"> expression regulier qui sert a faire le where de la requet select</param>
        /// <param name="OrderBy"> du type "OrderBy: c => c.OrderBy(d => d.property))"</param>
        /// <returns>retour un IQueryable sur base de la close donner dans filter et ordonner suivant le orderby</returns>
        public virtual IQueryable<TRetour> GetAll(
            Expression<Func<TRetour, bool>> Filter,
            Func<IQueryable<TRetour>, IOrderedQueryable<TRetour>> OrderBy
            )
        {
            List<TRetour> Retour = DbSet.Select(Item => EntityToRetour(Item)).ToList();

            IQueryable<TRetour> Query = Retour.AsQueryable();

            if (Filter != null)
            {
                Query = Query.Where(Filter);//applique le filtre a l'ensemble des entity compris dans query.
            }

            if (OrderBy != null)
            {
                Query = OrderBy(Query);//ordonne les entity compris dan la querry suivant la fonction orderby passé en parametre.
            }

            return Query;
        }

        /// <summary>
        /// fait un select sur la table
        /// </summary>
        /// <param name="Filter"> expression regulier qui sert a faire le where de la requet select</param>
        /// <returns>retour un IQueryable sur base de la close donner dans filter</returns>
        public virtual IQueryable<TRetour> GetAll(
            Expression<Func<TRetour, bool>> Filter
            )
        {
            List<TRetour> Retour = DbSet.Select(Item => EntityToRetour(Item)).ToList();

            IQueryable<TRetour> Query = Retour.AsQueryable();

            if (Filter != null)
            {
                Query = Query.Where(Filter);//applique le filtre a l'ensemble des entity compris dans query.
            }

            return Query;
        }

        /// <summary>
        /// fait un select sur la table
        /// </summary>
        /// <param name="OrderBy"> du type "OrderBy: c => c.OrderBy(d => d.property))"</param>
        /// <returns>retour un IQueryable ordonner suivant le orderby</returns>
        public virtual IQueryable<TRetour> GetAll(
            Func<IQueryable<TRetour>, IOrderedQueryable<TRetour>> OrderBy
            )
        {
            List<TRetour> Retour = DbSet.Select(Item => EntityToRetour(Item)).ToList();

            IQueryable<TRetour> Query = Retour.AsQueryable();

            if (OrderBy != null)
            {
                Query = OrderBy(Query);//ordonne les entity compris dan la querry suivant la fonction orderby passé en parametre.
            }

            return Query;
        }

        /// <summary>
        /// retourne une entity de la base de donner avec l'id donner 
        /// </summary>
        /// <param name="Id">l'ensemble des champs qui compose la primary key</param>
        /// <returns>retourne une entité</returns>
        public virtual TRetour GetOne(params object[] Id)
        {
            return EntityToRetour(DbSet.Find(Id));
        }
        /// <summary>
        /// Insert l'entity passer en parametre
        /// </summary>
        /// <param name="Entity">L'entity a inserer dans la db</param>
        public virtual void Insert(TEntity Entity)
        {
            DbSet.Add(Entity);
        }
        /// <summary>
        /// Insert l'entity passer en parametre de maniere local tant que l'on pas save
        /// </summary>
        /// <param name="Entity">L'entity a inserer dans la db</param>
        public virtual void Insert(TRetour Entity)
        {
            Insert(RetourToEntity(Entity));
        }
        /// <summary>
        /// Met a jours l'entité de maniere local tant que l'on pas save, la selectionne sur base des champs qui compose la clé primaire 
        /// </summary>
        /// <param name="Entity"> entré l'entity mit a jours sans avoir touchez au champs de clé primaire!</param>
        public virtual void Update(TEntity Entity)
        {
            DbSet.Attach(Entity);//erreur car cle primaire déja existant.
            Context.Entry(Entity).State = EntityState.Modified;
            //Context.SaveChanges();
        }

        /// <summary>
        /// Met a jours l'entité de maniere local tant que l'on pas save, la selection sur base des champs qui compose la clé primaire 
        /// </summary>
        /// <param name="Entity"> entré l'entity mit a jours sans avoir touchez au champs de clé primaire!</param>
        public virtual void Update(TRetour Entity)
        {
            Update(RetourToEntity(Entity));
        }

        protected virtual TEntity RetourToEntity(TRetour In)//assigne les mapper pour passé du type entity au type retour (a creer)
        {
            throw new NotImplementedException();
        }

        protected virtual TRetour EntityToRetour(TEntity In)//assigne les mapper pour passé du type retou au type entity (a creer)
        {
            throw new NotImplementedException();
        }
    }
}
