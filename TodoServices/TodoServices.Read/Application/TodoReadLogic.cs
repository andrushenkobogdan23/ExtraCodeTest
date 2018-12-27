using System.Linq;
using System.Collections.Generic;
using TodoServices.Domain;
using TodoServices.Domain.Model;
using TodoServices.Shared.Dto;
using Microsoft.EntityFrameworkCore;

namespace TodoServices.Read.Application
{
    /// <summary>
    /// возврат не доменной модели автоматом отключает отслеживание состояния и 
    /// доменная модель не помещается в кеш, что нам и нужно для сервиса чтения!
    /// </summary>
    internal class TodoReadLogic
    {
        private SqlDbContext _context = null;

        internal TodoReadLogic(SqlDbContext context)
        {
            _context = context;
        }

        internal IEnumerable<TodoInfo> Get()
        {          
           
            return ToInfo(_context.Todos);
        }


        private decimal ResetEstimatedCost(Todo t)
        {
            if (t.Childs != null)
            {
                foreach (var i in t.Childs)
                {
                    if (i != null)
                    {
                        t.EstimatedCost = t.EstimatedCost + i.EstimatedCost;
                    }
                }
            }
            return t.EstimatedCost;
        }

        private decimal? ResetCost(Todo t)
        {
            if (t.Childs != null)
            {
                foreach (var i in t.Childs)
                {
                    if (i != null)
                    {
                        t.Cost = t.Cost + i.Cost;
                    }
                }
            }
            return t.Cost;
        }

        internal TodoInfo Get(int id)
        {          

            return ToInfo(_context.Todos.Where(x => x.Id == id).Include(x => x.Childs)).FirstOrDefault();
        }

        internal IEnumerable<TodoInfo> My(int userId)
        {
            return ToInfo(_context.Todos.Where(x => x.AuthorId == userId));
        }

        internal IEnumerable<TodoInfo> Get(BaseFilter filter)
        {
            IQueryable<Todo> query = null;

            if (filter.PerformerId > 0)
                query = _context.Todos.Where(x => x.PerformerId == filter.PerformerId && x.CreateDate <= filter.To && x.CreateDate >= filter.From);
            else if(filter.PerformerId < 0)
                query = _context.Todos.Where(x => x.CreateDate <= filter.To && x.CreateDate >= filter.From);
            else
                query = _context.Todos.Where(x => x.CreateDate <= filter.To && x.CreateDate >= filter.From && x.Cost == null);
            

            foreach (var t in query)
            {
               t.EstimatedCost = ResetEstimatedCost(t);
            }
          

            if (filter.OnlyParent)
            {
                query = query.Where(x => !x.ParentId.HasValue);
            }

            return ToInfo(query.OrderBy(o => o.Id));
        }


        private TodoInfo ToInfo(Todo s)
        {
            return new TodoInfo
            {

                Childs =s.Childs?.Select(x => ToInfo(x)).ToList(),

                AuthorId = s.AuthorId,
                CompleteDate = s.CompleteDate,
                Cost =  s.Cost,
                AllCost = ResetCost(s),
                CreateDate = s.CreateDate,
                Description = s.Description,
                EstimatedCost =  s.EstimatedCost,
                AllEstimatedCost =ResetEstimatedCost(s),
                Id = s.Id,
                PerformerId = s.PerformerId,
                Priority = s.Priority,
                Title = s.Title,
                ParentId = s.ParentId
                               
            };
        }


        private IEnumerable<TodoInfo> ToInfo(IQueryable<Todo> query)
        {
            return query.AsEnumerable().Select(s => ToInfo(s));
        }
    }
}