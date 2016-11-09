using System;
using System.Data.Entity;

namespace ToolBoxPG.DAL
{
    /// <summary>
    /// Classe qui sert de base pour crée tout unit of work.
    /// </summary>
    public abstract class BaseUnitOfWork : IDisposable
    {
        protected DbContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DbContext">Défini la db qui sera utilise pour travaillé</param>
        protected BaseUnitOfWork(DbContext DbContext)
        {
            Context = DbContext;
        }


        public void Save()
        {
            Context.SaveChanges();
        }

        private bool _Disposed;

        protected virtual void Dispose(bool Disposing)
        {
            if (_Disposed)
            {
                if (Disposing)
                {
                    Context.Dispose();
                }
            }
            _Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
