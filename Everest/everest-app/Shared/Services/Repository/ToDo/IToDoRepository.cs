using System;
using everest_common.Models;

namespace everest_app.Shared.Services.Repository.ToDo
{
    public interface IToDoRepository
    {
        public abstract RepositoryResponseWrapper<List<ToDoItem>> ListToDoItems();
        public abstract Task<RepositoryResponseWrapper<List<ToDoItem>>> SaveToDoItem(ToDoItem toDoItem);
        public abstract Task<RepositoryResponseWrapper<List<ToDoItem>>> DeleteToDoItem(ToDoItem toDoItem);
    }
}

