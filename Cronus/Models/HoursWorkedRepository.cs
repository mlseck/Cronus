using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseEntities;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Cronus.Models
{
    public class HoursWorkedRepository : IHoursWorkedRepository
    {
        CronusDatabaseEntities context = HttpContext.Current.Items["CronusDatabaseEntities"] as CronusDatabaseEntities;

        public IQueryable<hoursworked> All
        {
            get { return context.hoursworkeds; }
        }

        public IQueryable<hoursworked> AllIncluding(params Expression<Func<hoursworked, object>>[] includeProperties)
        {
            IQueryable<hoursworked> query = context.hoursworkeds;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public hoursworked Find(int id)
        {
            return context.hoursworkeds.Find(id);
        }

        public void InsertOrUpdate(hoursworked hoursworked)
        {
            //if (hoursworked.hoursworkedID == default(int))
            if (Find(hoursworked.entryID) == null)
            {
                // New entity
                context.hoursworkeds.Add(hoursworked);
            }
            else
            {
                // Existing entity
                context.Entry(hoursworked).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var hoursworked = context.hoursworkeds.Find(id);
            context.hoursworkeds.Remove(hoursworked);
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

    public interface IHoursWorkedRepository : IDisposable
    {
        IQueryable<hoursworked> All { get; }
        IQueryable<hoursworked> AllIncluding(params Expression<Func<hoursworked, object>>[] includeProperties);
        hoursworked Find(int id);
        void InsertOrUpdate(hoursworked hoursworked);
        void Delete(int id);
        void Save();
    }
}