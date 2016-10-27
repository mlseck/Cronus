using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseEntities;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Cronus.Models
{
    public class ProjectRepository: IProjectRepository
    {
        CronusDatabaseEntities context = HttpContext.Current.Items["CronusDatabaseEntities"] as CronusDatabaseEntities;

        public IQueryable<project> All
        {
            get { return context.projects; }
        }

        public IQueryable<project> AllIncluding(params Expression<Func<project, object>>[] includeProperties)
        {
            IQueryable<project> query = context.projects;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public project Find(int id)
        {
            return context.projects.Find(id);
        }

        public void InsertOrUpdate(project project)
        {
            //if (project.projectID == default(int))
            if (Find(project.projectID) == null)
            {
                // New entity
                context.projects.Add(project);
            }
            else
            {
                // Existing entity
                context.Entry(project).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var project = context.projects.Find(id);
            context.projects.Remove(project);
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

    public interface IProjectRepository : IDisposable
    {
        IQueryable<project> All { get; }
        IQueryable<project> AllIncluding(params Expression<Func<project, object>>[] includeProperties);
        project Find(int id);
        void InsertOrUpdate(project project);
        void Delete(int id);
        void Save();
    }
}