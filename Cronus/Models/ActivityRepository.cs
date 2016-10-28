using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseEntities;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Cronus.Models
{
    public class ActivityRepository: IActivityRepository
    {
        CronusDatabaseEntities context = HttpContext.Current.Items["CronusDatabaseEntities"] as CronusDatabaseEntities;

        public IQueryable<activity> All
        {
            get { return context.activities; }
        }

        public IQueryable<activity> AllIncluding(params Expression<Func<activity, object>>[] includeProperties)
        {
            IQueryable<activity> query = context.activities;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public activity Find(int id)
        {
            return context.activities.Find(id);
        }

        public void InsertOrUpdate(activity activity)
        {
            if (Find(activity.activityID) == null)
            {
                // New entity
                context.activities.Add(activity);
            }
            else
            {
                // Existing entity
                context.Entry(activity).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var activity = context.activities.Find(id);
            context.activities.Remove(activity);
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

    public interface IActivityRepository : IDisposable
    {
        IQueryable<activity> All { get; }
        IQueryable<activity> AllIncluding(params Expression<Func<activity, object>>[] includeProperties);
        activity Find(int id);
        void InsertOrUpdate(activity activity);
        void Delete(int id);
        void Save();
    }
}