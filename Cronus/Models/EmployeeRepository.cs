using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseEntities;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Cronus.Models
{
    public class EmployeeRepository : IemployeeRepository
    {
        CronusDatabaseEntities context = HttpContext.Current.Items["CronusDatabaseEntities"] as CronusDatabaseEntities;

        public IQueryable<employee> All
        {
            get { return context.employees; }
        }

        public IQueryable<employee> AllIncluding(params Expression<Func<employee, object>>[] includeProperties)
        {
            IQueryable<employee> query = context.employees;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public employee Find(string id)
        {
            return context.employees.Find(id);
        }

        public void InsertOrUpdate(employee employee)
        {
            if (Find(employee.employeeID) == null)
            {
                // New entity
                context.employees.Add(employee);
            }
            else
            {
                // Existing entity
                context.Entry(employee).State = EntityState.Modified;
            }
        }

        public void Delete(string id)
        {
            var employee = context.employees.Find(id);
            context.employees.Remove(employee);
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

    public interface IemployeeRepository : IDisposable
    {
        IQueryable<employee> All { get; }
        IQueryable<employee> AllIncluding(params Expression<Func<employee, object>>[] includeProperties);
        employee Find(string id);
        void InsertOrUpdate(employee employee);
        void Delete(string id);
        void Save();
    }
}