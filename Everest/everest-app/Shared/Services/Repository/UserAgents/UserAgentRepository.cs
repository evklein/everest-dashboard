﻿using System;
using everest_app.Data;
using everest_app.Shared.Services.Repository.Tags;
using everest_common.Models;
using Microsoft.AspNetCore.Identity;

namespace everest_app.Shared.Services.Repository.UserAgents
{
    public class UserAgentRepository : IUserAgentRepository
    {
        private readonly ApplicationDbContext _everestDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITagRepository _tagRepository;

        public UserAgentRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager, ITagRepository tagRepository)
        {
            _everestDbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _tagRepository = tagRepository;
        }

        public RepositoryResponseWrapper<List<UserAgent>> GetUserAgents()
        {
            var userAgents = _everestDbContext.UserAgents.ToList();

            return new RepositoryResponseWrapper<List<UserAgent>>()
            {
                Value = userAgents,
            };
        }

        public async Task<RepositoryResponseWrapper<List<UserAgent>>> AddUserAgent(UserAgent userAgent)
        {
            var existingUserAgent = await _everestDbContext.UserAgents.FindAsync(userAgent.Id);

            if (existingUserAgent is not null)
            {
                return new RepositoryResponseWrapper<List<UserAgent>>()
                {
                    Success = false,
                    Error = new RepositoryResponseError()
                    {
                        ErrorMessage = $"Error adding UserAgent: UserAgent with ID {userAgent.Id} already found.",
                    },
                };
            }

            await _everestDbContext.UserAgents.AddAsync(userAgent);
            await _everestDbContext.SaveChangesAsync();

            return GetUserAgents();
        }

        //public async Task<RepositoryResponseWrapper<List<ToDoItem>>> SaveToDoItem(ToDoItem toDoItem, ToDoHistoricalDisplayPolicy displayPolicy)
        //{
        //    var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

        //    if (string.IsNullOrEmpty(toDoItem.Name))
        //    {
        //        return new RepositoryResponseWrapper<List<ToDoItem>>()
        //        {
        //            Success = false,
        //            Error = new RepositoryResponseError()
        //            {
        //                ErrorMessage = "Error saving To-Do item: invalid name",
        //            },
        //        };
        //    }

        //    try
        //    {
        //        var syncedTagList = await _tagRepository.AddNewTags(toDoItem.Tags.ToList());

        //        var existingToDoItem = await _everestDbContext.ToDoItems.FindAsync(toDoItem.Id);

        //        if (existingToDoItem is not null)
        //        {
        //            if (!existingToDoItem.OwnerId.Equals(currentUser.Id))
        //            {
        //                return new RepositoryResponseWrapper<List<ToDoItem>>()
        //                {
        //                    Success = false,
        //                    Error = new RepositoryResponseError()
        //                    {
        //                        ErrorMessage = "Error saving new To-Do item: you are not the owner of this item",
        //                    },
        //                };
        //            }

        //            existingToDoItem.Complete = toDoItem.Complete;
        //            existingToDoItem.DateCompleted = toDoItem.Complete ? DateTime.UtcNow : DateTime.MinValue;
        //            existingToDoItem.Tags = syncedTagList;
        //        }
        //        else
        //        {
        //            toDoItem.OwnerId = currentUser.Id;
        //            toDoItem.Tags = syncedTagList;
        //            await _everestDbContext.ToDoItems.AddAsync(toDoItem);
        //        }
        //        await _everestDbContext.SaveChangesAsync();
        //        return await ListToDoItems(displayPolicy);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new RepositoryResponseWrapper<List<ToDoItem>>()
        //        {
        //            Success = false,
        //            Error = new RepositoryResponseError()
        //            {
        //                ErrorMessage = "Error saving new To-Do item: unknown exception",
        //                InnerException = ex,
        //            },
        //        };
        //    }
        //}

    }
}

