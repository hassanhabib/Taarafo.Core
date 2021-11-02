// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Posts;

namespace Taarafo.Core.Services.Foundations.Posts
{
    public interface IPostService
    {
        ValueTask<Post> AddPostAsync(Post post);
        ValueTask<Post> RetrievePostByIdAsync(Guid postId);
        IQueryable<Post> RetrieveAllPosts();
        ValueTask<Post> ModifyPostAsync(Post post);
        ValueTask<Post> RemovePostByIdAsync(Guid postId);
    }
}
