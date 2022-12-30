// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.Posts;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Post> Posts { get; set; }

        public async ValueTask<Post> InsertPostAsync(Post post) =>
            await InsertAsync(post);

        public IQueryable<Post> SelectAllPosts() =>
            SelectAll<Post>();

        public async ValueTask<Post> SelectPostByIdAsync(Guid postId) =>
            await SelectAsync<Post>(postId);

        public async ValueTask<Post> UpdatePostAsync(Post post) =>
            await UpdateAsync(post);

        public async ValueTask<Post> DeletePostAsync(Post post) =>
            await DeleteAsync(post);
    }
}
