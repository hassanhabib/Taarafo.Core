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
        ValueTask<Post> InsertPostAsync(Post post);
        ValueTask<Post> UpdatePostAsync(Post post);
        ValueTask<Post> SelectPostByIdAsync(Guid postId);
        IQueryable<Post> SelectAllPosts();
        ValueTask<Post> DeletePostAsync(Post post);



    }
}
