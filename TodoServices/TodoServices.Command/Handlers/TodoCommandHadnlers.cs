using System;
using System.Threading.Tasks;
using TodoServices.Domain.Model;
using TodoServices.Shared.Events;
using TodoServices.Command.Commands;
using Shared.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Common.Sql;
using TodoServices.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TodoServices.Command.Handlers
{
    /// <summary>
    /// містить логіку обробки команд
    /// </summary>
    internal class TodoCommandHadnlers
    {
        const string error_text = "wrong value";
        private readonly IUnitOfWork _work;

        internal TodoCommandHadnlers(IUnitOfWork work)
        {
            _work = work;
        }

        internal async Task Handle(TodoAddCommand msg, ModelStateDictionary modelState)
        {
            if (!IsStateValid(msg, modelState))
                return;

            var todo = new Todo{
                AuthorId = msg.AuthorId,
                CreateDate = DateTime.Now,
                Description = msg.Description,
                EstimatedCost = msg.EstimatedCost,
                Priority = msg.Priority,
                Title = msg.Title,
                ParentId = msg.ParentId,
               
            };

            _work.Repository.Insert(todo);

            await _work.SaveAsync();

            await _work.PublishEventAsync<TodoAddedEvent>(new TodoAddedEvent (todo.Id, todo.AuthorId, todo.Title ));

            msg.Id = todo.Id;
        }


        internal async Task Handle(TodoUpdateCommand msg, ModelStateDictionary modelState)
        {
            if (!IsStateValid(msg, modelState))
                return;

            var item = HasItemCompleted(msg.Id);

            var oldAuthor = item.AuthorId;

            item.AuthorId = msg.AuthorId;
            item.Title = msg.Title;
            item.EstimatedCost = msg.EstimatedCost;
            item.Title = msg.Title;
            item.Description = msg.Description;
            item.Priority = msg.Priority;
            item.ParentId = msg.ParentId;

            // update
            _work.Repository.Update(item);
            await _work.SaveAsync();

            //if (oldAuthor != msg.AuthorId) todo: this is subscriber logic, not publisher!!!
            await _work.PublishEventAsync<TodoUpdatedEvent>(new TodoUpdatedEvent (msg.Id, oldAuthor, msg.AuthorId, msg.EstimatedCost, msg.Title,msg.ParentId));

        }

        internal async Task Handle(TodoCompleteCommand msg, ModelStateDictionary modelState)
        {
            if (!(msg.Cost > 0))
            {
                modelState.AddModelError(nameof(msg.Cost), error_text);
                return;
            }

            var item = HasItemCompleted(msg.Id);

            if (CheckToFinishChilds(item))
            {
                modelState.AddModelError(nameof(msg.Id), "You must finish all child tasks");
                return;
            }
            


            item.CompleteDate = DateTime.Now;
            item.Cost = msg.Cost;
            item.PerformerId = msg.PerformerId;
            

            // update
            _work.Repository.Update(item);

            await _work.SaveAndPublish<TodoCompletedEvent>(new TodoCompletedEvent(msg.Id, item.Title, msg.PerformerId, msg.Cost));
        }

        internal async Task Handle(int id, int userId)
        {
            var item = HasItemCompleted(id);           

            foreach(var ch in item.Childs)
            {
                _work.Repository.Delete(ch);
            }
            
            // drop
            _work.Repository.Delete(item);
            await _work.SaveAndPublish<TodoDeletedEvent>(new TodoDeletedEvent(id, userId));
        }
 

        private bool CheckToFinishChilds(Todo todo)
        {
            bool isChilds = false;
            if (todo.Childs != null)
            {
                foreach (var ch in todo.Childs)
                {
                    if (ch != null)
                    {
                        if (ch.Cost == null)
                        {
                            isChilds = true;
                        }
                    }
                }
            }

            return isChilds;
        }


        private Todo HasItemCompleted(int id)
        {
            var todo = FindRecord(id);
            HasItemCompleted(todo);                    
            return todo;
        }

        private void HasItemCompleted(Todo item)
        {
            if (item.CompleteDate.HasValue && item.PerformerId > 0)
                throw new AggregateReadonlyException(item.Id);
        }


        private Todo FindRecord(int id)
        {
            var context = (SqlDbContext)_work.Repository.Context;
            var item = context.Todos.Where(x => x.Id == id).Include(x => x.Childs).FirstOrDefault();

            // var item = _work.Repository.GetById<Todo, int>(id);

            if (item == null)
                throw new AggregateNotFoundException(id);

            return item;
        }

        private bool IsStateValid(ITodoCheckFields msg, ModelStateDictionary modelState)
        {
           
            if (!(msg.AuthorId > 0))
                modelState.AddModelError(nameof(Todo.AuthorId), error_text);

            if (msg.EstimatedCost == 0)
                modelState.AddModelError(nameof(Todo.EstimatedCost), error_text);

            if (string.IsNullOrEmpty(msg.Title) || msg.Title.Length > 32)
                modelState.AddModelError(nameof(Todo.Title), error_text);

            if (string.IsNullOrEmpty(msg.Description) || msg.Description.Length > 32 || msg.Description.Length < 5 )
                modelState.AddModelError(nameof(Todo.Description), error_text);

            return modelState.ErrorCount == 0;
        }
    }
}
