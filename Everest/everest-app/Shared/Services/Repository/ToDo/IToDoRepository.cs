using System;
using everest_common.Enumerations;
using everest_common.Models;

namespace everest_app.Shared.Services.Repository.ToDo
{
    public interface IToDoRepository
    {
        public abstract Task<RepositoryResponseWrapper<List<ToDoItem>>> ListToDoItems(ToDoHistoricalDisplayPolicy displayPolicy);
        public abstract Task<RepositoryResponseWrapper<List<ToDoItem>>> SaveToDoItem(ToDoItem toDoItem, ToDoHistoricalDisplayPolicy displayPolicy);
        public abstract Task<RepositoryResponseWrapper<List<ToDoItem>>> DeleteToDoItem(ToDoItem toDoItem, ToDoHistoricalDisplayPolicy displayPolicy);
    }
}

