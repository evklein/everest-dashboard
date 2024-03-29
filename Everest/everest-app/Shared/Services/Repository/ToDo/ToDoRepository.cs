﻿using System;
using everest_app.Data;
using everest_app.Shared.Services.Repository.Tags;
using everest_common.Enumerations;
using everest_common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace everest_app.Shared.Services.Repository.ToDo
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ApplicationDbContext _everestDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITagRepository _tagRepository;

        public ToDoRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager, ITagRepository tagRepository)
        {
            _everestDbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _tagRepository = tagRepository;
        }

        public async Task<RepositoryResponseWrapper<List<ToDoItem>>> ListToDoItems(ToDoHistoricalDisplayPolicy displayPolicy)
        {
            DateTime availableDateMinimum = convertHistoricalDisplayPolicyIntoDate(displayPolicy);
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

            try
            {
                var toDoItems = _everestDbContext.ToDoItems.Include(t => t.Tags)
                                                    .Where(t => t.OwnerId.Equals(currentUser.Id) &&
                                                                !t.Complete || t.Complete &&
                                                                t.DateCompleted > availableDateMinimum)
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
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

            if (string.IsNullOrEmpty(toDoItem.Name))
            {
                return new RepositoryResponseWrapper<List<ToDoItem>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = "Error saving To-Do item: invalid name",
                    },
                };
            }

            try
            {
                var syncedTagList = await _tagRepository.AddNewTags(toDoItem.Tags.ToList());

                var existingToDoItem = await _everestDbContext.ToDoItems.FindAsync(toDoItem.Id);

                if (existingToDoItem is not null)
                {
                    if (!existingToDoItem.OwnerId.Equals(currentUser.Id))
                    {
                        return new RepositoryResponseWrapper<List<ToDoItem>>()
                        {
                            Success = false,
                            Error = new RepositoryResponseError()
                            {
                                ErrorMessage = "Error saving new To-Do item: you are not the owner of this item",
                            },
                        };
                    }

                    existingToDoItem.Complete = toDoItem.Complete;
                    existingToDoItem.DateCompleted = toDoItem.Complete ? DateTime.UtcNow : DateTime.MinValue;
                    existingToDoItem.Tags = syncedTagList;
                }
                else
                {
                    toDoItem.OwnerId = currentUser.Id;
                    toDoItem.Tags = syncedTagList;
                    await _everestDbContext.ToDoItems.AddAsync(toDoItem);
                }
                await _everestDbContext.SaveChangesAsync();
                return await ListToDoItems(displayPolicy);
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
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
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
                if (!toDoItemToDelete.OwnerId.Equals(currentUser.Id))
                {
                    return new RepositoryResponseWrapper<List<ToDoItem>>()
                    {
                        Success = false,
                        Error = new RepositoryResponseError()
                        {
                            ErrorMessage = "Error deleting To-Do item: you are not the owner of this item",
                        },
                    };
                }
                _everestDbContext.ToDoItems.Remove(toDoItem);
                await _everestDbContext.SaveChangesAsync();
                return await ListToDoItems(displayPolicy);
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

