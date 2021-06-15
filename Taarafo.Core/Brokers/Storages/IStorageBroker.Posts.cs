// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Posts;

namespace Taarafo.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<Post> InsertPostAsync(Post post);
        public IQueryable<Post> SelectAllPosts();
        public ValueTask<Post> SelectPostByIdAsync(Guid postId);
        public ValueTask<Post> UpdatePostAsync(Post post);
        public ValueTask<Post> DeletePostAsync(Post post);
    }
}
