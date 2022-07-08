using System;
using everest_app.Data;
using everest_common.Models;

namespace everest_app.Shared.Services.Repository.ToDo
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ApplicationDbContext _everestDbContext;

        public ToDoRepository(ApplicationDbContext dbContext)
        {
            _everestDbContext = dbContext;
        }

        public RepositoryResponseWrapper<List<ToDoItem>> ListToDoItems()
        {
            try
            {
                var toDoItems = _everestDbContext.ToDoItems.ToList();
                toDoItems.ForEach(todoItem => todoItem.UpdatedName = todoItem.Name);
                return new RepositoryResponseWrapper<List<ToDoItem>>()
                {
                    Value = toDoItems,
                };
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<List<ToDoItem>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error getting ToDo items: unknown exception",
                        InnerException = ex,
                    },
                };
            }
        }

        public async Task<RepositoryResponseWrapper<List<ToDoItem>>> SaveToDoItem(ToDoItem toDoItem)
        {
            try
            {
                await _everestDbContext.ToDoItems.AddAsync(toDoItem);
                return ListToDoItems();
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<List<ToDoItem>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error saving new To-Do item: unknown exception",
                        InnerException = ex,
                    },
                };
            }
        }

        public async Task<RepositoryResponseWrapper<List<ToDoItem>>> DeleteToDoItem(ToDoItem toDoItem)
        {
            ToDoItem toDoItemToDelete = await _everestDbContext.ToDoItems.FindAsync(toDoItem.Id);

            if (toDoItemToDelete is null)
            {
                return new RepositoryResponseWrapper<List<ToDoItem>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error deleting To-Do item: item could not be found",
                    },
                };
            }

            try
            {
                _everestDbContext.ToDoItems.Remove(toDoItem);
                await _everestDbContext.SaveChangesAsync();
                return ListToDoItems();
            }
            catch (Exception ex)
            {
                return new RepositoryResponseWrapper<List<ToDoItem>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error deleting To-Do item: Unexpected exception",
                        InnerException = ex,
                    },
                };
            }
        }
    }
}

