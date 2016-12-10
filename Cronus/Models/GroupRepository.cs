using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseEntities;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Cronus.Models
{
    public class GroupRepository : IGroupRepository
    {
        CronusDatabaseEntities context = HttpContext.Current.Items["CronusDatabaseEntities"] as CronusDatabaseEntities;

        public IQueryable<group> All
        {
            get { return context.groups; }
        }

        public IQueryable<group> AllIncluding(params Expression<Func<group, object>>[] includeProperties)
        {
            IQueryable<group> query = context.groups;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public group FindGroup(int id)
        {
            return context.groups.Find(id);
        }

        public employee FindEmployee(string id)
        {
            return context.employees.Find(id);
        }

        public void InsertOrUpdate(group group, employee manager)
        {
            //if (group.groupID == default(int))
            if (FindGroup(group.groupID) == null)
            {
                // New entity
                context.groups.Add(group);
            }
            else
            {
                // Existing entity
                context.Entry(group).State = EntityState.Modified;
            }

            if (manager != null && FindEmployee(manager.employeeID) != null)
            {
                // Existing entity
                context.Entry(manager).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var group = context.groups.Find(id);
            context.groups.Remove(group);
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

    public interface IGroupRepository : IDisposable
    {
        IQueryable<group> All { get; }
        IQueryable<group> AllIncluding(params Expression<Func<group, object>>[] includeProperties);
        group FindGroup(int id);
        void InsertOrUpdate(group group, employee employee);
        void Delete(int id);
        void Save();
    }
}