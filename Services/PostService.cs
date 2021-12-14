using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProjectAircraftForum.Data;
using WebProjectAircraftForum.Models.Forum;
using WebProjectAircraftForum.Models.Post;
using WebProjectAircraftForum.Models.PostReply;

namespace WebProjectAircraftForum.Services
{
    public class PostService : IPost
    {
        private readonly WebProjectAircraftForumDbContext _context;
        public PostService(WebProjectAircraftForumDbContext context)
        {
            _context = context;
        }

        public async Task Add(Post post)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task AddReply(PostReply reply)
        {
            _context.PostReplies.Add(reply);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var post = GetById(id);
            _context.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostReply(int id)
        {
            var reply = GetReplyById(id);
            _context.Remove(reply);
            await _context.SaveChangesAsync();
        }

        public async Task EditPostContent(int id, string newContent)
        {
            var post = GetById(id);
            post.Content = newContent;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task EditPostReplyContent(int id, string newContent)
        {
            var reply = GetReplyById(id);
            reply.Content = newContent;
            reply.CreatedOn = DateTime.Now;
            _context.PostReplies.Update(reply);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts
                .Include(post => post.User)
                .Include(post => post.Replies).ThenInclude(reply => reply.User)
                .Include(post => post.Forum);
        }

        public Post GetById(int id)
        {
            return _context.Posts.Where(post => post.Id == id)
                .Include(post => post.User)
                .Include(post => post.Replies).ThenInclude(reply => reply.User)
                .Include(post => post.Forum)
                .FirstOrDefault();
        }

        public PostReply GetReplyById(int id)
        {
            return _context.PostReplies.Where(reply => reply.Id == id)
                .Include(reply => reply.Post).ThenInclude(post => post.Forum)
                .Include(reply => reply.Post).ThenInclude(post => post.User)
                .FirstOrDefault();
        }

        public IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery)
        {
                return string.IsNullOrEmpty(searchQuery) ? forum.Posts : forum.Posts.Where(post => post.Title.Contains(searchQuery) || post.Content.Contains(searchQuery));
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return GetAll().OrderByDescending(post => post.CreatedOn).Take(10);
            }
            else
            {
                var toLower = searchQuery.ToLower();
                return GetAll().Where(post => post.Title.ToLower().Contains(toLower) || post.Content.ToLower().Contains(toLower));
            }
        }

        public IEnumerable<Post> GetLatestPosts(int n)
        {
            return GetAll().OrderByDescending(post => post.CreatedOn).Take(n);
        }

        public IEnumerable<Post> GetPostsByForum(int id)
        {
            var posts = _context.Forums.Where(forum => forum.Id == id).First().Posts;

            return posts;
        }

    }
}
