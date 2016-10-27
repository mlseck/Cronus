using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseEntities;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Cronus.Models
{
    public class FavoriteRepository : IFavoriteRepository
    {
        CronusDatabaseEntities context = HttpContext.Current.Items["CronusDatabaseEntities"] as CronusDatabaseEntities;

        public IQueryable<favorite> All
        {
            get { return context.favorites; }
        }

        public IQueryable<favorite> AllIncluding(params Expression<Func<favorite, object>>[] includeProperties)
        {
            IQueryable<favorite> query = context.favorites;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public favorite Find(int id)
        {
            return context.favorites.Find(id);
        }

        public void InsertOrUpdate(favorite favorite)
        {
            //if (favorite.favoriteID == default(int))
            if (Find(favorite.favoriteID) == null)
            {
                // New entity
                context.favorites.Add(favorite);
            }
            else
            {
                // Existing entity
                context.Entry(favorite).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var favorite = context.favorites.Find(id);
            context.favorites.Remove(favorite);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }

    public interface IFavoriteRepository : IDisposable
    {
        IQueryable<favorite> All { get; }
        IQueryable<favorite> AllIncluding(params Expression<Func<favorite, object>>[] includeProperties);
        favorite Find(int id);
        void InsertOrUpdate(favorite favorite);
        void Delete(int id);
        void Save();
    }
}