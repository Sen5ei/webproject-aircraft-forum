﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProjectAircraftForum.Areas.Identity.Data;
using WebProjectAircraftForum.Data;
using WebProjectAircraftForum.Models;
using WebProjectAircraftForum.Models.Forum;

namespace WebProjectAircraftForum.Services
{
    public class ForumService : IForum
    {
        private readonly WebProjectAircraftForumDbContext _context;
        public ForumService(WebProjectAircraftForumDbContext context)
        {
            _context = context;
        }

        public async Task Create(Forum forum)
        {
            _context.Add(forum);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int forumId)
        {
            var forum = GetById(forumId);
            _context.Remove(forum);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(int forumId, string newTitle, string newDescription, string newImageUrl)
        {
            var forumToEdit = GetById(forumId);
            forumToEdit.Title = newTitle;
            forumToEdit.Description = newDescription;
            forumToEdit.ImageUrl = newImageUrl;
            _context.Forums.Update(forumToEdit);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<ApplicationUser> GetActiveUsers(int id)
        {
            var posts = GetById(id).Posts;

            if (posts != null || !posts.Any())
            {
                var postUsers = posts.Select(p => p.User);
                var replyUsers = posts.SelectMany(p => p.Replies).Select(r => r.User);

                var users = postUsers.Union(replyUsers).Distinct();

                return users;
            }

            return new List<ApplicationUser>();
        }

        public IEnumerable<Forum> GetAll()
        {
            return _context.Forums
                .Include(forum => forum.Posts);
        }

        public Forum GetById(int id)
        {
            var forum = _context.Forums.Where(f => f.Id == id)
                .Include(f => f.Posts).ThenInclude(p => p.User)
                .Include(f => f.Posts).ThenInclude(p => p.Replies).ThenInclude(r => r.User)
                    .FirstOrDefault();

            return forum;
        }

        public bool HasRecentPost(int id)
        {
            const int hoursAgo = 12;
            var window = DateTime.Now.AddHours(-hoursAgo);

            return GetById(id).Posts.Any(post => post.CreatedOn > window);
        }
    }
}
