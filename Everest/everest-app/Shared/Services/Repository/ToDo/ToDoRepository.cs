using System;
using everest_app.Data;
using everest_common.Enumerations;
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

        public RepositoryResponseWrapper<List<ToDoItem>> ListToDoItems(ToDoHistoricalDisplayPolicy displayPolicy)
        {
            DateTime availableDateMinimum = convertHistoricalDisplayPolicyIntoDate(displayPolicy);
            try
            {
                var toDoItems = _everestDbContext.ToDoItems
                    .Where(t => !t.Complete || t.Complete && t.DateCompleted > availableDateMinimum)
                    .OrderByDescending(t => t.DateCreated).ToList();
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

        public async Task<RepositoryResponseWrapper<List<ToDoItem>>> SaveToDoItem(ToDoItem toDoItem, ToDoHistoricalDisplayPolicy displayPolicy)
        {
            try
            {
                var existingToDoItem = await _everestDbContext.ToDoItems.FindAsync(toDoItem.Id);

                if (existingToDoItem is not null)
                {
                    existingToDoItem.Complete = toDoItem.Complete;
                    existingToDoItem.DateCompleted = toDoItem.Complete ? DateTime.UtcNow : DateTime.MinValue;
                }
                else
                {
                    await _everestDbContext.ToDoItems.AddAsync(toDoItem);
                }
                await _everestDbContext.SaveChangesAsync();
                return ListToDoItems(displayPolicy);
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

        public async Task<RepositoryResponseWrapper<List<ToDoItem>>> DeleteToDoItem(ToDoItem toDoItem, ToDoHistoricalDisplayPolicy displayPolicy)
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
                return ListToDoItems(displayPolicy);
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

        private DateTime convertHistoricalDisplayPolicyIntoDate(ToDoHistoricalDisplayPolicy displayPolicy) =>
             displayPolicy switch
             {
                 ToDoHistoricalDisplayPolicy.OneDay => DateTime.UtcNow.AddDays(-1),
                 ToDoHistoricalDisplayPolicy.ThreeDays => DateTime.UtcNow.AddDays(-3),
                 ToDoHistoricalDisplayPolicy.OneWeek => DateTime.UtcNow.AddDays(-7),
                 ToDoHistoricalDisplayPolicy.OneMonth => DateTime.UtcNow.AddMonths(-1),
                 ToDoHistoricalDisplayPolicy.OneYear => DateTime.UtcNow.AddYears(-1),
                 ToDoHistoricalDisplayPolicy.All => DateTime.MinValue,
             };
    }
}

